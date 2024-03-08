using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public SoundEffect footsteps;
    // For character animation
    public Animator _playerAnimator;
    public GameControls gameControls;
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
        if (gameControls.Wrapper.Player.Move.IsPressed())
        {
            _playerAnimator.SetBool("isWalking", true);

            if (gameControls.Wrapper.Player.Sprint.IsPressed())
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
