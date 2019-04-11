using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsPlayer : MonoBehaviour
{
    private Vector2 pointDOWN, pointUP;
    public Vector2 direction;

    private PlayerMovement playerMovement;
    private PlayerController2D playerController2D;

    private Collider2D colliderP;

    void Start()
    {
        playerMovement = GameObject.Find("Franki").GetComponent<PlayerMovement>();
        colliderP = GetComponent<Collider2D>();
        playerController2D = playerMovement.GetComponent<PlayerController2D>();

        Vector2 pointUp = GetComponent<Transform>().Find("PointUp").position;
        Vector2 pointDown = GetComponent<Transform>().Find("PointDown").position;

        direction = (pointUp - pointDown).normalized;


        if (Vector2.Angle(Vector2.right, direction) > 90)
            direction *= -1;


        Destroy(GetComponent<Transform>().Find("PointDown").gameObject);
        Destroy(GetComponent<Transform>().Find("PointUp").gameObject);
    }

    void Update()
    {
        /*if (playerController2D.spaceB)
            colliderP.enabled = true;
        else if(playerController2D.isGrounded && false)
                colliderP.enabled = false;*/


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (playerController2D.CheckGroundedAgain())
            playerController2D.isStairs = true;

            int c = 0;
            if(playerController2D.rightB)
            {
                c = -1;
            }
            if (playerController2D.leftB)
            {
                c = 1;
            }


            playerMovement.dopDir = -direction * Time.deltaTime * c * 10;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            
        }
    }
}
