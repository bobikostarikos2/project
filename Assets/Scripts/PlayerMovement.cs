using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 lastPos;
    private int side_definition, dirCamera = 1;

    public Vector2 speedPlayer;

    private PlayerController2D playerController2D;
    private Rigidbody2D rb_2D;
    private CameraTarget cameraTarget;

    private SpriteRenderer spriteRenderer;

    public Vector2 dopDir, smoothCameraX;

    void Start()
    {
        playerController2D = GetComponent<PlayerController2D>();
        rb_2D = GetComponent<Rigidbody2D>();
        cameraTarget = playerController2D.cameraTarget;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public void Move(Vector2 movement)
    {
        rb_2D.MovePosition(rb_2D.position + movement + dopDir);
        dopDir = Vector2.zero;

        if (speedPlayer.x > 0.1f)
            dirCamera = -1;
        if (speedPlayer.x < -0.1f)
            dirCamera = 1;


        smoothCameraX = Vector2.Lerp(smoothCameraX, Vector2.up * 2 + Vector2.right * 10 * dirCamera, Time.deltaTime * 5);

        cameraTarget.GoalPursuit(transform.position + (Vector3)smoothCameraX);

    }


    private float rotationZ = 0;

    public void PhysicsPlayer(Vector2 movement, bool play)
    {
        rb_2D.simulated = play;
        if (play)
        {
            Vector2 curPos = (Vector2)transform.position - dopDir;

            speedPlayer.x = (lastPos.x - curPos.x) - (lastPos.y - curPos.y) * 0;
            speedPlayer.y = (lastPos.y - curPos.y);

            if (speedPlayer.y < 0)
            {
                speedPlayer.y = 0;
            }

            if (speedPlayer.x < 0)
                side_definition = -1;
            if (speedPlayer.x > 0)
                side_definition = 1;

            Move(movement);

            lastPos = transform.position;

            float speedY = speedPlayer.y < 0.1f ? speedPlayer.y : 0.1f;

            if (speedPlayer.x > 0 && playerController2D.leftB)
                spriteRenderer.flipX = true;
            else if (speedPlayer.x < 0 && playerController2D.rightB)
                spriteRenderer.flipX = false;


            


            //Определение угла поверхности
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5);
            Quaternion rotNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);


            RaycastHit2D hitR = Physics2D.Raycast(transform.position + Vector3.right, Vector2.down, 10);

            float angleBetweenPlane = Quaternion.Angle(Quaternion.identity, rotNormal);


            if (hitR.point.y < hit.point.y)
                angleBetweenPlane *= -1;
            rotationZ = Mathf.MoveTowards(rotationZ, angleBetweenPlane, Time.deltaTime * 100);

            transform.eulerAngles = new Vector3(0, 0, rotationZ);
        }
    }
}
