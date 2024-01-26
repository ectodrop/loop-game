using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _controller;
    private const float DefaultSpeed = 2.0f;
    private float _playerSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _controller.Move(gameObject.transform.forward * _playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _controller.Move(-gameObject.transform.forward * _playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _controller.Move(-gameObject.transform.right * _playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _controller.Move(gameObject.transform.right * _playerSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerSpeed = DefaultSpeed * 2;
        }
        else
        {
            _playerSpeed = DefaultSpeed;
        }
    }
}