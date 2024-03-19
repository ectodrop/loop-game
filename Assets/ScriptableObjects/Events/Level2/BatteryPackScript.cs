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

    private bool _draining;

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
        batteryDisabled.TriggerEvent();
        enableBatteryDrain.TriggerEvent();
    }

    private void HandleBatteryOff()
    {
        _batteryIn = false;
        batteryEnabled.TriggerEvent();
    }

    private void Update()
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
                batteryEnabled.TriggerEvent();
                disableBatteryDrain.TriggerEvent();
                batteryPackCharged.TriggerEvent();
            }
        }
        else if (_draining)
        {
            if (bar.value > 0f)
            {
                bar.value -= Time.deltaTime;
            }
            else
            {
                bar.value = 0f;
                if (_batteryIn)
                {
                    enableBatteryDrain.TriggerEvent();
                    batteryPackDepleted.TriggerEvent();
                }
            }
        }
    }

    private bool IsFull()
    {
        return bar.value >= timeToChargeSeconds;
    }

    public void DrainBatteryPack()
    {
        _draining = true;
    }
}
