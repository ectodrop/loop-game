using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTime : MonoBehaviour
{
    // Player's rigidbody
    Rigidbody rb;

    // Player speed
    [SerializeField] float speed = 3f;
    [SerializeField] float jumpForce = 5;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // obtain rigidbody one time at the start of the game
    }

    // Update is called once per frame
    void Update()
    {   
        // horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);

        // jump
        if (Input.GetButtonDown("Jump")){
            jump();
        }
    }

    void jump() {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
  
}
