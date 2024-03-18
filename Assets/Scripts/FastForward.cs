using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FastForward : MonoBehaviour
{
    public GameControls gameControls;

    [Header("Sets Shared Variables")]
    public SharedBool timeStoppedFlag;

    private void OnEnable()
    {
        gameControls.Wrapper.Player.FastForward.performed += HandleFastForwardStart;
        gameControls.Wrapper.Player.FastForward.canceled += HandleFastForwardStop;
    }

    private void OnDisable()
    {
        gameControls.Wrapper.Player.FastForward.performed -= HandleFastForwardStart;
        gameControls.Wrapper.Player.FastForward.canceled -= HandleFastForwardStop;
    }

    public void HandleFastForwardStart(InputAction.CallbackContext _)
    {
        if (!timeStoppedFlag.GetValue())
        {
            Time.timeScale = 2f; // Start fast-forwarding
            gameControls.Wrapper.Player.Move.Disable();
            gameControls.Wrapper.Player.Look.Disable();
            gameControls.Wrapper.Player.Sprint.Disable();
            gameControls.Wrapper.Player.Jump.Disable();
            gameControls.Wrapper.Player.Interact.Disable();
            // gameControls.Wrapper.Player.TimeStop.Disable();
        }
    }

    public void HandleFastForwardStop(InputAction.CallbackContext _)
    {
        if (!timeStoppedFlag.GetValue())
        {
            Time.timeScale = 1f; // Stop fast-forwarding and return to normal time scale
            gameControls.Wrapper.Player.Look.Enable();
            gameControls.Wrapper.Player.Move.Enable();
            gameControls.Wrapper.Player.Sprint.Enable();
            gameControls.Wrapper.Player.Jump.Enable();
            gameControls.Wrapper.Player.Interact.Enable();
            // gameControls.Wrapper.Player.TimeStop.Enable();
        }
    }
}
