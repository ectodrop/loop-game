using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public SoundEffect footsteps;
    // For character animation
    public Animator _playerAnimator;
    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
    }

    public void PlayFootSteps()
    {
        footsteps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _playerAnimator.SetBool("isWalking", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _playerAnimator.SetFloat("walkingSpeed", 2f);
            } else
            {
                _playerAnimator.SetFloat("walkingSpeed", 1f);
            }
            
        }
        else
        {
            _playerAnimator.SetFloat("walkingSpeed", 0f);
            _playerAnimator.SetBool("isWalking", false);
        }
    }
}
