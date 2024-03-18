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
        if (timeStoppedFlag.GetValue())
        {
            return; // Do nothing if time is stopped
        }

        Time.timeScale = 2f; // Start fast-forwarding
    }

    public void HandleFastForwardStop(InputAction.CallbackContext _)
    {
        if (timeStoppedFlag.GetValue())
        {
            return; // Do nothing if time is stopped
        }

        Time.timeScale = 1f; // Stop fast-forwarding and return to normal time scale
    }
}
