using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool disabled = false;

    public CharacterController controller;
    public float speed = 12f;
    public float sprintSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled) { 
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            //Debug.Log("isgrounded: " + isGrounded);
            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (velocity.y < 200)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);
            if (Input.GetKey(KeyCode.T))
            {
            }
        }
    }
}
