using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [System.Serializable]
    public struct Areas
    {
        [SerializeField]
        public Vector2 areaLeft, areaRight, areaDown, areaUp;
    };

    public bool play = true;
    public float maxSpeed, radius, rayCircleLength, rayLength,
        jump_force, gravity_force, gravityWater = 1f, moveWater = 1f;
    public bool isGrounded, landing, landing_True;
    public Vector2 respawnPosition;

    [SerializeField]
    public Areas areas;


    private Vector2 control_coordinate;
    private float m_movement;

    public bool inWater;

    private PlayerMovement playerMovement;
    private AudioSource audioSource;
    private Rigidbody2D rb_2D;
    private CircleCollider2D circleCollider2D;

    public CameraTarget cameraTarget;
    public AudioClip jumpSound;


    public bool respawnPlayer, isStairs;

    public bool spaceB, leftB, rightB;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        landing = true;

        rb_2D = GetComponent<Rigidbody2D>();

        respawnPosition = transform.position;
    }

    public bool CheckGroundedAgain()
    {
        return CheckGrounded(new Vector2(transform.position.x, transform.position.y - circleCollider2D.radius * transform.localScale.x), areas.areaDown / 100, true);
    }

    private bool CheckGrounded(Vector2 sensorPoint, Vector2 sensorAria, bool checkStairs)
    {
        //Vector2 sensorPos = new Vector2(transform.position.x, transform.position.y - circleCollider2D.radius / 10);
        //Collider2D[] colliders2D = Physics2D.OverlapCircleAll(sensorPos, sensor_radius);
        Collider2D[] colliders2D = Physics2D.OverlapBoxAll(sensorPoint, sensorAria, 0);

        int m_L = 0;
        if (checkStairs)
            for (int i = 0; i < colliders2D.Length; i++)
            {
                if (colliders2D[i].transform.tag == "Stairs")
                {
                    m_L -= 1;
                }
            }

        return colliders2D.Length + m_L > 0;
    }

    void FixedUpdate()
    {
        //RaycastHit2D hitCircle = Physics2D.CircleCast(transform.position, radius, -Vector2.up, rayCircleLength);
        /*RaycastHit2D hitRayRight = Physics2D.Raycast(transform.position, Vector2.right, rayLength);
        RaycastHit2D hitRayLeft = Physics2D.Raycast(transform.position, -Vector2.right, rayLength);
        RaycastHit2D hitRayUp = Physics2D.CircleCast(transform.position, radius, Vector2.up, rayCircleLength);
        RaycastHit2D hitRayDown = Physics2D.CircleCast(transform.position, radius, -Vector2.up, rayCircleLength);*/

        bool hitLeft = CheckGrounded(new Vector2(transform.position.x - circleCollider2D.radius* transform.localScale.x, transform.position.y), areas.areaLeft.normalized, false);
        bool hitRight = CheckGrounded(new Vector2(transform.position.x + circleCollider2D.radius * transform.localScale.x, transform.position.y), areas.areaRight, false);
        bool hitUp = CheckGrounded(new Vector2(transform.position.x, transform.position.y + circleCollider2D.radius * transform.localScale.x), areas.areaUp, false);

        //isGrounded = hitRayDown.collider != null;
        isGrounded = CheckGrounded(new Vector2(transform.position.x, transform.position.y - circleCollider2D.radius * transform.localScale.x), areas.areaDown, false);

        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            spaceB = true;
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
            spaceB = false;


        if (isGrounded)
        {
            if (!inWater)
                moveWater = 1f;
            if (landing)
            {
                control_coordinate.y = -0f;
                if (spaceB)
                {
                    if (play)
                    {
                        control_coordinate.y = jump_force * Time.deltaTime * gravityWater;
                        landing = false;

                        audioSource.clip = jumpSound;
                        // audioSource.Play();
                    }
                }
            }
            else if (landing_True)
            {
                landing = true;
                landing_True = false;
            }
        }
        else
        {
            landing_True = true;
        }

        if (!landing || !isGrounded)
        {
            control_coordinate.y -= gravity_force * Time.deltaTime * gravityWater;
        }



        leftB = Input.GetAxisRaw("Horizontal") == -1 ? true : false;
        rightB = Input.GetAxisRaw("Horizontal") == 1 ? true : false;

        m_movement = 0;

        if (leftB)
            m_movement = -1;
        if (rightB)
            m_movement = 1;
        if (leftB && rightB)
            m_movement = 0;

            control_coordinate.x = Mathf.MoveTowards(control_coordinate.x, m_movement * maxSpeed / 10 * moveWater, Time.deltaTime);
        if (control_coordinate.x < 0 && hitLeft || control_coordinate.x > 0 && hitRight)
        {
            control_coordinate.x = 0;
        }
        if (hitUp && false)
        {
            if (control_coordinate.y > 0)
            {
                control_coordinate.y = 0;
            }
        }
        if (isStairs)
        {
            control_coordinate *= 0;
            isStairs = false;
        }

        playerMovement.PhysicsPlayer(control_coordinate, play);

        if (respawnPlayer)
        {
            Respawn();
            respawnPlayer = false;
        }

    }




    public void Respawn()
    {
        control_coordinate = Vector2.zero;
        transform.position = respawnPosition;
        play = true;
    }

    /* 3 - jump
       1 - left
       2 - right
    */
    public void InputMove(int dir)
    {
        switch (dir)
        {
            case 1: leftB = true; break;
            case 2: rightB = true; break;
            case 3: spaceB = true; break;

            case -1: leftB = false; break;
            case -2: rightB = false; break;
            case -3: spaceB = false; break;

            default: break;
        }
    }
}




