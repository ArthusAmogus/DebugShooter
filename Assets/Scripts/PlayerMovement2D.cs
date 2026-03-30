using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement2D : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private bool pressedRight;
    [SerializeField] private bool pressedLeft;
    [SerializeField] private bool pressedJump;

    [SerializeField] private bool readyToJump;



    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("PlayerMovement2D: No Rigidbody2D assigned. Please assign a Rigidbody2D component.");
            }
        }

    }

    //Input System
    void Update()
    {
        //Key Downs
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { pressedLeft = true; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { pressedRight = true; }
        if (Input.GetKeyDown(KeyCode.Z) && readyToJump) { pressedJump = true; }
        


        //Key Ups
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { pressedLeft = false; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { pressedRight = false; }

    }

    //Physics System
    private void FixedUpdate()
    {
        if (pressedLeft) { rb.AddForce(Vector2.left * moveSpeed); }
        if (pressedRight) { rb.AddForce(Vector2.right * moveSpeed); }


        if (pressedJump)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            readyToJump=false;
            pressedJump=false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            float groundDistance = transform.position.y - collision.transform.position.y;
            if (groundDistance>1)
            {
                readyToJump = true;
            }
        }
    }
}
