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
    public bool freezeTimeAtStart = false;
    public int numTimeStops = 0;
    public TimeSettings timeSettings;
    public ScheduleControllerScriptableObject scheduleController;

    private int currentEvent = 0;
    private float timestopCooldown = 1.2f;
    private float lastTimestop = 0f;

    [Header("Listening To")]
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public GameEventInt timeExtendedEvent;

    [Header("Triggers")]
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;
    public GameEvent resetLoopEvent;
    public GameEventString setWarningTextEvent;
    public GameEvent flashWarningTextEvent;
    public GameEvent fadeWarningTextEvent;

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
    }

    private void OnEnable()
    {
        timeExtendedEvent.AddListener(HandleTimeExtension);
        gameControls.Wrapper.Player.TimeStop.performed += HandleTimeStop;
    }

    private void OnDisable()
    {
        timeExtendedEvent.RemoveListener(HandleTimeExtension);
        gameControls.Wrapper.Player.TimeStop.performed -= HandleTimeStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame && freezeTimeAtStart)
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
        if (Time.time - lastTimestop < timestopCooldown)
            return;
        
        if (!timeStoppedFlag.GetValue())
        {
            if (numTimeStops <= 0)
            {
                setWarningTextEvent.TriggerEvent("Time Stops Remaining: 0");
                flashWarningTextEvent.TriggerEvent();
                return;
            }
            timestopStartSFX.Play();
            numTimeStops--;
            StopTime();
            timeStopStartEvent.TriggerEvent();
        }
        else
        {
            setWarningTextEvent.TriggerEvent($"Time Stops Remaining: {numTimeStops}");
            fadeWarningTextEvent.TriggerEvent();
            timestopEndSFX.Play();
            ResumeTime();
            timeStopEndEvent.TriggerEvent();
        }

        lastTimestop = Time.time;
    }

    public void StopTime()
    {
        timeStoppedFlag.SetValue(true);
        Shader.SetGlobalInteger("_TimeStopped", 1);
    }

    public void ResumeTime()
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
}