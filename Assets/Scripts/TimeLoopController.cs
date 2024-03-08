using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class TimeLoopController : MonoBehaviour
{
    public GameControls gameControls;
    public bool debugMode = false;
    public TimeSettings timeSettings;
    public ScheduleControllerScriptableObject scheduleController;

    private int currentEvent = 0;
    private float timestopCooldown = 1.2f;
    private float lastTimestop = 0f;

    // Handle battery usage
    private bool _usingBattery = false;

    [Header("Listening To")]
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public GameEventInt timeExtendedEvent;

    [Header("Triggers")]
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    public GameEvent resetLoopEvent;

    [Header("Sets Shared Variables")]
    public SharedBool timeStoppedFlag;
    
    [Header("Sound Effects")]
    public SoundEffect timeloopEndSFX;
    public SoundEffect timestopStartSFX;
    public SoundEffect timestopEndSFX;

    private bool firstFrame = true;
    private void Start()
    {
        timeSettings.ResetTimers();
        timeStoppedFlag.ResetValue();
        
    }

    private void OnEnable()
    {
        batteryDraining.AddListener(HandleBatteryDraining);
        batteryStopDraining.AddListener(HandleBatteryStoppedDraining);
        timeExtendedEvent.AddListener(HandleTimeExtension);
        gameControls.Wrapper.Player.TimeStop.performed += HandleTimeStop;
    }

    private void OnDisable()
    {
        batteryDraining.RemoveListener(HandleBatteryDraining);
        batteryStopDraining.RemoveListener(HandleBatteryStoppedDraining);
        timeExtendedEvent.RemoveListener(HandleTimeExtension);
        gameControls.Wrapper.Player.TimeStop.performed -= HandleTimeStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
            StopTime();
            timeStopStartEvent.TriggerEvent();
        }       

        if (timeStoppedFlag.GetValue() || debugMode)
        {
            return;
        }

        if (timeSettings.currentTimeSeconds < timeSettings.CurrentMaxTimeSeconds())
        {
            int prevNextIncrement = timeSettings.NextIncrement();
            timeSettings.IncrementSeconds(Time.deltaTime);
            int nextIncrement = timeSettings.NextIncrement();
            if (prevNextIncrement < nextIncrement)
            {
                timeSettings.currentTimestamp.AddMinutes(timeSettings.minutesPerIncrement);
            }
            InvokeNextEvent();
        }
        else
        {
            StopTime();
            timeloopEndSFX.Play();
            resetLoopEvent.TriggerEvent();
        }
    }

    private void HandleTimeStop(InputAction.CallbackContext _)
    {
        if (lastTimestop > 0)
            return;
        
        if (Time.time - lastTimestop < timestopCooldown)
            return;
        
        if (!timeStoppedFlag.GetValue())
        {
            timestopStartSFX.Play();
            StopTime();
            timeStopStartEvent.TriggerEvent();
        }
        else
        {
            timestopEndSFX.Play();
            ResumeTime();
            timeStopEndEvent.TriggerEvent();
        }

        lastTimestop = Time.time;
    }

    private void StopTime()
    {
        timeStoppedFlag.SetValue(true);
        Shader.SetGlobalInteger("_TimeStopped", 1);
    }

    private void ResumeTime()
    {
        timeStoppedFlag.SetValue(false);
        Shader.SetGlobalInteger("_TimeStopped", 0);
        
    }

    private void InvokeNextEvent()
    {
        if (currentEvent >= scheduleController.scheduledEvents.Length)
            return;

        var nextEvent = scheduleController.scheduledEvents[currentEvent];
        if (nextEvent.time.CompareTo(timeSettings.currentTimestamp) <= 0)
        {
            nextEvent.TriggerEvent();
            currentEvent++;
        }
    }

    private void HandleTimeExtension(int addedTime)
    {
        timeSettings.currentStartTimestamp.AddMinutes(-addedTime);
        resetLoopEvent.TriggerEvent();
    }

    private void HandleBatteryDraining()
    {
        _usingBattery = true;
    }

    private void HandleBatteryStoppedDraining()
    {
        _usingBattery = false;
    }
}