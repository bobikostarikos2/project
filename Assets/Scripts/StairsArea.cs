using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsArea : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D;
    private Collider2D col2D;
    private PlayerController2D playerController2D;


    private StairsPlayer stairsPlayer;
    void Start()
    {
        stairsPlayer = transform.parent.GetComponent<StairsPlayer>();
        col2D = stairsPlayer.GetComponent<Collider2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();

        playerController2D = GameObject.Find("Franki").GetComponent<PlayerController2D>();

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.tag == "Player")
        {
            if(!playerController2D.isGrounded)
            {
                col2D.enabled = true;

            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.tag == "Player")
        {
            col2D.enabled = false;
        }
    }
}
