using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public SoundEffect playerZapSFX;
    private CharacterController _controller;
    [SerializeField] private float DefaultSpeed = 2.0f;
    private float _playerSpeed = 2.0f;
    private Vector3 _velocity;

    // For jumping
    private bool _grounded;
    private const float JumpHeight = 1.0f; // Adjust as needed
    private const float Gravity = -9.81f;
    
    // For BounceBack
    private bool _bounce = false;
    private Vector3 BounceDirection = new Vector3(0, 0, 0);
    private float BounceDuration = 0.1f;
    private float BounceTimer = 0.1f;
    



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

        // BounceBack
        if (_bounce)
        {
            _velocity += BounceDirection * 5 * _playerSpeed;

            BounceTimer -= Time.deltaTime;
            if (BounceTimer <= 0.0f)
            {
                Debug.Log("time 0");
                _bounce = false;
                BounceTimer = BounceDuration;
            }
        }

        // ----------------------------------------------------------
        // Update ALL movement after compute
        _controller.Move(_velocity * Time.deltaTime);
    }
    // When player contacts the electrified puddle, the player will bounce back.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("puddle"))
        {
            playerZapSFX.Play();
            _bounce = true;
            BounceDirection = new Vector3(-hit.moveDirection.x, 0, -hit.moveDirection.z);
        }
    }
}