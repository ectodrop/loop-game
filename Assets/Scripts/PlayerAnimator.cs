using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public SoundEffect footsteps;
    // For character animation
    public Animator _playerAnimator;

    private string _starting_foot;
    private bool isWalking = false;

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
        if (!isWalking)
        {
            if (pressedWalking())
            {
                if (Random.Range(0f, 1.0f) < 0.5f)
                {
                    _starting_foot = "isWalking";
                }
                else
                {
                    _starting_foot = "isWalkingRight";
                }
                _playerAnimator.SetBool(_starting_foot, true);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _playerAnimator.SetFloat("walkingSpeed", 2f);
                }
                else
                {
                    _playerAnimator.SetFloat("walkingSpeed", 1f);
                }
                isWalking = true;
            }
        } else
        {
            if (!pressedWalking())
            {
                _playerAnimator.SetFloat("walkingSpeed", 0f);
                _playerAnimator.SetBool(_starting_foot, false);
                isWalking = false;
            }
        }
        
    }

    bool pressedWalking()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }
}
