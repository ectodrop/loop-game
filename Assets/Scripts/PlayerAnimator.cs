using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public SoundEffect footsteps;
    // For character animation
    public Animator _playerAnimator;
    public GameControls gameControls;

    private string _starting_foot = "isWalkingLeft";
    private bool _isWalking = false;

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
        if (!_isWalking)
        {
            if (pressedWalking())
            {
                if (Random.Range(0f, 1.0f) < 0.5f)
                {
                    _starting_foot = "isWalkingLeft";
                }
                else
                {
                    _starting_foot = "isWalkingRight";
                }

                _playerAnimator.SetBool(_starting_foot, true);

                if (gameControls.Wrapper.Player.Sprint.IsPressed())
                {
                    _playerAnimator.SetFloat("walkingSpeed", 2f);
                } else
                {
                    _playerAnimator.SetFloat("walkingSpeed", 1f);
                }
                _isWalking = true;
            }
        } else
        {
            if (!pressedWalking())
            {
                _playerAnimator.SetFloat("walkingSpeed", 0f);
                _playerAnimator.SetBool(_starting_foot, false);
                _isWalking = false;
            }
        }
    }

    bool pressedWalking()
    {
        return gameControls.Wrapper.Player.Move.IsPressed();
    }
}
