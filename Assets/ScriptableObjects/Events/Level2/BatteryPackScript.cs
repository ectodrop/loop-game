using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPackScript : MonoBehaviour
{
    public Slider bar;
    public float timeToChargeSeconds = 1f;
    public SharedBool timeStoppedFlag;
    public HintData batteryChargerHint;
    [Header("Listening To")]
    public GameEvent batteryOn;
    public GameEvent batteryOff;

    [Header("Triggers")]
    public GameEvent disableBatteryDrain;
    public GameEvent enableBatteryDrain;
    public GameEvent batteryPackCharged;
    public GameEvent batteryPackDepleted;
    public GameEvent batteryEnabled;
    public GameEvent batteryDisabled;

    private bool _batteryIn;


    private void Start()
    {
        bar.maxValue = timeToChargeSeconds;
    }

    private void OnEnable()
    {
        batteryOn.AddListener(HandleBatteryOn);
        batteryOff.AddListener(HandleBatteryOff);
    }

    private void OnDisable()
    {
        batteryOn.RemoveListener(HandleBatteryOn);
        batteryOff.RemoveListener(HandleBatteryOff);
    }

    private void HandleBatteryOn()
    {
        _batteryIn = true;
        enableBatteryDrain.TriggerEvent();
    }

    private void HandleBatteryOff()
    {
        _batteryIn = false;
        batteryChargerHint.Unlock();
    }

    private void Update()
    {
        if (!timeStoppedFlag.GetValue())
        {
            if (_batteryIn)
            {
                if (bar.value < timeToChargeSeconds)
                {
                    bar.value += Time.deltaTime;
                }
                else
                {
                    bar.value = timeToChargeSeconds;
                    batteryPackCharged.TriggerEvent();
                }
            }
            else
            {
                if (bar.value > 0f)
                {
                    bar.value -= Time.deltaTime/10;
                }
                else
                {
                    bar.value = 0f;
                    batteryPackDepleted.TriggerEvent();
                }
            }
        }
    }
}
