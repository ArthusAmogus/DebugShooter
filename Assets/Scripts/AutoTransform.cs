using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransform : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private float PosX;
    [SerializeField] private float PosY;
    [SerializeField] private float PosZ;

    [Header("Rotation")]
    [SerializeField] private float RotX;
    [SerializeField] private float RotY;
    [SerializeField] private float RotZ;

    [Header("Scale")]
    [SerializeField] private float ScaX;
    [SerializeField] private float ScaY;
    [SerializeField] private float ScaZ;


    [Header("Properties")]
    [SerializeField] private bool DoMove = true;
    [SerializeField] private bool DoRotate = true;
    [SerializeField] private bool DoScale = true;
    
    
    void Update()
    {
        if (DoRotate)
        {
            transform.Rotate(
                RotX * Time.deltaTime,
                RotY * Time.deltaTime,
                RotZ * Time.deltaTime
                );
        }

        if (DoMove)
        {
            transform.position = new(
                transform.position.x + (PosX * Time.deltaTime),
                transform.position.y + (PosX * Time.deltaTime),
                transform.position.z + (PosX * Time.deltaTime)
                );
        }

        if (DoScale)
        {
            transform.localScale = new(
                transform.localScale.x + (ScaX * Time.deltaTime),
                transform.localScale.y + (ScaY * Time.deltaTime),
                transform.localScale.z + (ScaZ * Time.deltaTime)
                );
        }
    }
}
