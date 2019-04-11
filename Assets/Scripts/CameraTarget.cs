using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    public Transform target;
    public float speed = 10f;
    public bool block_pos;
    public Vector3 posFixed;

    void Start()
    {

    }

    void LateUpdate()
    {
        /*if (!block_pos) {
			Vector3 pointA = transform.position;
			Vector3 pointB = new Vector3 (target.position.x, target.position.y, transform.position.z);

			Vector3 vectorAB = pointB - pointA;

			float controlSpeed = Time.deltaTime * speed;
			if (Vector3.Distance (pointA, pointB) > controlSpeed) {
				transform.position = vectorAB * controlSpeed + pointA;
			} else {
				transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
			}
		} else {
			transform.position = Vector3.MoveTowards (transform.position, posFixed, Time.deltaTime * 5);
		}*/
    }

    public void GoalPursuit(Vector3 target)
    {
        if (!block_pos)
        {
            /*Vector3 pointA = transform.position;
            Vector3 pointB = new Vector3(target.x, target.y, transform.position.z);

            Vector3 vectorAB = pointB - pointA;

            float controlSpeed = Time.deltaTime * speed;
            if (Vector3.Distance(pointA, pointB) > controlSpeed * Time.deltaTime * 100)
            {
                //transform.position = vectorAB * controlSpeed * Time.deltaTime * 100 + pointA;
            }
            else
            {
                //transform.position = new Vector3(target.x, target.y, transform.position.z);
            }
            */
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, -10), Time.deltaTime * speed);
        }
        else
        {
            //target.z = -10;
            //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            FixedPosture(posFixed);
        }
    }

    public void FixedPosture(Vector3 target)
    {
        target.z = -10;
        transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime * speed / 5);
    }

    private void FixedUpdate()
    {

    }
}
