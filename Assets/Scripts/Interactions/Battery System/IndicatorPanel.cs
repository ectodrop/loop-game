using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPanel : MonoBehaviour
{
    // LEDS
    public GameObject powerLed;

    public GameObject generatorLed;

    public GameObject batteryLed;

    private bool _usingGenerator = true;
    
    [Header("Listening To")]
    public GameEvent switchOn;
    public GameEvent switchOff;
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public ScheduleEvent powerOutage;

    [Header("Triggers")]
    // Power event
    public GameEvent powerOn;
    public GameEvent powerOff;



    void Start()
    {
        // LED defaults
        SetGreenColor(powerLed);
        SetGreenColor(generatorLed);
        SetRedColor(batteryLed);
    }

    private void OnEnable()
    {
        switchOn.AddListener(HandleSwitchOn);
        switchOff.AddListener(HandleSwitchOff);
        batteryDraining.AddListener(HandleBatteryDrain);
        batteryStopDraining.AddListener(HandleBatteryDrainStop);
        powerOutage.AddListener(HandlePowerOutage);
    }

    private void OnDisable()
    {
        switchOn.RemoveListener(HandleSwitchOn);
        switchOff.RemoveListener(HandleSwitchOff);
        batteryDraining.RemoveListener(HandleBatteryDrain);
        batteryStopDraining.RemoveListener(HandleBatteryDrainStop);
        powerOutage.RemoveListener(HandlePowerOutage);
    }


    private void HandleSwitchOn()
    {
        SetGreenColor(batteryLed);
        SetRedColor(generatorLed);
        _usingGenerator = false;
    }

    private void HandleSwitchOff()
    {
        SetRedColor(batteryLed);
        SetGreenColor(generatorLed);
        _usingGenerator = true;
    }

    private void HandleBatteryDrain()
    {
        SetGreenColor(powerLed);
    }

    private void HandleBatteryDrainStop()
    {
        SetRedColor(powerLed);
    }

    private void HandlePowerOutage()
    {
        if (_usingGenerator)
        {
            SetRedColor(powerLed);
        }
    }

    private void SetRedColor(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        if (obj == powerLed)
        {
            powerOff.TriggerEvent();
        }
    }

    private void SetGreenColor(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
        if (obj == powerLed)
        {
            powerOn.TriggerEvent();
        }
    }
}