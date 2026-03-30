using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float RotationSpeed=10;
    public Vector3 RotationOffset;
    public bool DisableX;
    public bool DisableY;
    public bool DisableZ;
    public bool doLook;
    private float ogspeed;

    private void Start()
    {
        ogspeed = RotationSpeed;
    }

    void Update()
    {
        if (doLook)
        {
            if (target!=null)
            {
                Vector3 direction = target.position - transform.position;
                if (DisableX) direction.x = 0;
                if (DisableY) direction.y = 0;
                if (DisableZ) direction.z = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    Quaternion finalRotation = lookRotation * Quaternion.Euler(RotationOffset);

                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        finalRotation,
                        Time.deltaTime * RotationSpeed
                    );
                }
            }
            
        }
    }

    public void SetSpeed(float speed)
    {
        RotationSpeed = speed;
    }

    public void ResetSpeed()
    {
        RotationSpeed = ogspeed;
    }

    public void SetLook(Transform pos)
    {
        target = pos;
    }
}
