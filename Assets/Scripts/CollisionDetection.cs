using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public string TagToDetect;
    public bool DoDetection;
    public GameObject DetectedObject;
    public bool collided;



    //Collision
    private void OnCollisionStay(Collision collision)
    {
        if (DoDetection)
        {
            if (TagToDetect.Equals(""))
            {
                collided = true;
                DetectedObject = collision.gameObject;
            }
            else if (collision.gameObject.CompareTag(TagToDetect))
            {
                collided = true;
                DetectedObject = collision.gameObject;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collided = false;
        DetectedObject = null;
    }



    //Trigger
    

    private void OnTriggerStay(Collider other)
    {
        if (DoDetection)
        {
            if (TagToDetect.Equals(""))
            {
                collided = true;
                DetectedObject = other.gameObject;
            }
            else if (other.gameObject.CompareTag(TagToDetect))
            {
                collided = true;
                DetectedObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (DoDetection)
        {
            if (other.gameObject.CompareTag(TagToDetect))
            {
                collided = false;
                DetectedObject = null;
            }
        }
    }


}
