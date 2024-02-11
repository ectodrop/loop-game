using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float DefaultSpeed = 2.0f;
    private float _playerSpeed = 2.0f;
    private Vector3 _velocity;

    // For jumping
    private bool _grounded;
    private const float JumpHeight = 1.0f; // Adjust as needed
    private const float Gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Movement
        // ----------------------------------------------------------
        // Set to 0 velocity in x and z if no button is being pushed
        _velocity.x = 0f;
        _velocity.z = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            _velocity += transform.forward * _playerSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _velocity -= transform.forward * _playerSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _velocity -= transform.right * _playerSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _velocity += transform.right * _playerSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = DefaultSpeed * 2;
        }
        else
        {
            _playerSpeed = DefaultSpeed;
        }

        // Jumping
        // ----------------------------------------------------------
        // Check if the player is on the ground (Adjust as needed)
        _grounded = Physics.Raycast(transform.position, Vector3.down, _controller.height / 2 + 0.1f);

        // Gravity
        if (!_grounded)
        {
            _velocity.y += Gravity * Time.deltaTime;
        }
        else if (_velocity.y < 0)
        {
            _velocity.y = 0f;
        }

        // Jump trigger
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _velocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
        }

        // ----------------------------------------------------------
        // Update ALL movement after compute
        _controller.Move(_velocity * Time.deltaTime);
    }
}