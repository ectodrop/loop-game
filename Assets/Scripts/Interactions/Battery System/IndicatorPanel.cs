using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IndicatorPanel : MonoBehaviour
{
    // LEDS
    public GameObject powerLed;

    public GameObject generatorLed;

    public GameObject batteryLed;

    private bool _usingGenerator = true;

    // Is power being drawn from the battery
    private bool _usingBattery = false;

    // Is there a power outage?
    private bool _powerOutage = false;

    [Header("Listening To")]
    public GameEvent switchOn;
    public GameEvent switchOff;
    public GameEvent batteryDraining;
    public GameEvent batteryStopDraining;
    public ScheduleEvent powerOutageEvent;

    [Header("Triggers")]
    // Power event
    public GameEvent powerOn;
    public GameEvent powerOff;

    [Header("Sound Effects")]
    public SoundEffect powerOffSFX;

    public SoundEffect powerOnSFX;
    
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
        powerOutageEvent.AddListener(HandlePowerOutage);
    }

    private void OnDisable()
    {
        switchOn.RemoveListener(HandleSwitchOn);
        switchOff.RemoveListener(HandleSwitchOff);
        batteryDraining.RemoveListener(HandleBatteryDrain);
        batteryStopDraining.RemoveListener(HandleBatteryDrainStop);
        powerOutageEvent.RemoveListener(HandlePowerOutage);
    }


    // Use battery power
    private void HandleSwitchOn()
    {
        SetGreenColor(batteryLed);
        SetRedColor(generatorLed);

        // If no battery then we should not have power
        if (!_usingBattery)
        {
            TriggerPowerOff(powerLed);
        }

        _usingGenerator = false;
    }

    // Use generator power
    private void HandleSwitchOff()
    {
        SetRedColor(batteryLed);
        SetGreenColor(generatorLed);

        // If no power outage has occured yet then we should still have power
        if (!_powerOutage)
        {
            TriggerPowerOn(powerLed);
        }

        _usingGenerator = true;
    }

    private void HandleBatteryDrain()
    {
        TriggerPowerOn(powerLed);
        _usingBattery = true;
    }

    private void HandleBatteryDrainStop()
    {
        // If we are not using generator power then no power
        // OR if there's a power outage
        if (!_usingGenerator || _powerOutage)
        {
            TriggerPowerOff(powerLed);
        }

        _usingBattery = false;
    }

    private void HandlePowerOutage()
    {
        // If we are using the generator or the battery is not being drained then no power
        if (_usingGenerator || !_usingBattery)
        {
            TriggerPowerOff(powerLed);
        }

        _powerOutage = true;
    }

    private void TriggerPowerOff(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        powerOffSFX.Play();
        powerOff.TriggerEvent();
    }

    private void TriggerPowerOn(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
        powerOnSFX.Play();
        powerOn.TriggerEvent();
    }


    private void SetRedColor(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
    }

    private void SetGreenColor(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
    }
}