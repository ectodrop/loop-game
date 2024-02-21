using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class TimeLoopController : MonoBehaviour
{
    public bool debugMode = false;
    public TimeSettings timeSettings;
    public ScheduleControllerScriptableObject scheduleController;

    private int currentEvent = 0;
    private float timestopCooldown = 2.0f;
    private float timestopCooldownTimer = 0.0f;

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

    private void Start()
    {
        timeSettings.ResetTimers();
        timeStoppedFlag.ResetValue();
        ResumeTime();
    }

    private void OnEnable()
    {
        batteryDraining.AddListener(HandleBatteryDraining);
        batteryStopDraining.AddListener(HandleBatteryStoppedDraining);
        timeExtendedEvent.AddListener(HandleTimeExtension);
    }

    private void OnDisable()
    {
        batteryDraining.RemoveListener(HandleBatteryDraining);
        batteryStopDraining.RemoveListener(HandleBatteryStoppedDraining);
        timeExtendedEvent.RemoveListener(HandleTimeExtension);
    }

    // Update is called once per frame
    void Update()
    {
        if (timestopCooldownTimer < timestopCooldown)
            timestopCooldownTimer += Time.deltaTime;
        
        if (timestopCooldownTimer >= timestopCooldown && Input.GetKeyDown(KeyCode.R))
        {
            if (!timeStoppedFlag.GetValue())
            {
                timestopCooldownTimer = 0.0f;
                StopTime();
                timeStopStartEvent.TriggerEvent();
            }
            else
            {
                timestopCooldownTimer = 0.0f;
                ResumeTime();
                timeStopEndEvent.TriggerEvent();
            }
        }
        
        if (timeStoppedFlag.GetValue() || debugMode)
        {
            return;
        }

        if (timeSettings.currentTimeSeconds < timeSettings.CurrentMaxTimeSeconds())
        {
            int prevNextIncrement = timeSettings.NextIncrement();
            timeSettings.currentTimeSeconds += Time.deltaTime;
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
            resetLoopEvent.TriggerEvent();
        }
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