using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FastForward : MonoBehaviour
{
    public SoundEffect fastForwardSFX;
    public TimeSettings timeSettings;
    public GameControls gameControls;
    public float timeScale = 2f;

    public SharedBool timeStoppedFlag;
    [Header("Triggers")]
    public GameEvent fastforwardStartEvent;
    public GameEvent fastforwardEndEvent;

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
        if (!timeStoppedFlag.GetValue() && Time.timeScale > 0)
        {
            fastForwardSFX.Play();
            fastforwardStartEvent.TriggerEvent();
            Time.timeScale = timeSettings.fastForwardTimeScale; // Start fast-forwarding
            gameControls.Wrapper.Player.Move.Disable();
            gameControls.Wrapper.Player.Look.Disable();
            gameControls.Wrapper.Player.Sprint.Disable();
            gameControls.Wrapper.Player.Jump.Disable();
            gameControls.Wrapper.Player.Interact.Disable();
            gameControls.Wrapper.Player.TimeStop.Disable();
        }
    }

    public void HandleFastForwardStop(InputAction.CallbackContext _)
    {
        if (!timeStoppedFlag.GetValue() && Time.timeScale > 0)
        {
            fastForwardSFX.Stop();
            fastforwardEndEvent.TriggerEvent();
            Time.timeScale = 1f; // Stop fast-forwarding and return to normal time scale
            gameControls.Wrapper.Player.Look.Enable();
            gameControls.Wrapper.Player.Move.Enable();
            gameControls.Wrapper.Player.Sprint.Enable();
            gameControls.Wrapper.Player.Jump.Enable();
            gameControls.Wrapper.Player.Interact.Enable();
            gameControls.Wrapper.Player.TimeStop.Enable();
        }
    }
}
