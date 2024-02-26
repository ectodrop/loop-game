using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Tutorial Flow
 * 1. User tries door button, power outage, enable power switch
 * 2. Once user switches, disable switch, enable battery
 * 3. User can open the door with battery inserted
 */
public class TutorialGuide : MonoBehaviour
{
    public TextMeshProUGUI playerThoughts;
    public ScheduleEvent powerOutageEvent;

    // Event Tracking
    public GameEvent doorFirstClick;
    private bool _clickedSwitch = false;

    [Header("Triggers")]
    // Enable and disable battery pickup
    public GameEvent enableBattery;

    public GameEvent disableBattery;

    // Enable and disable switch function
    public GameEvent enableSwitch;
    public GameEvent disableSwitch;

    [Header("Listening To")]
    public GameEvent switchOn;
    public GameEvent powerOn;


    public void Start()
    {
        // Disable Battery and Switch
        disableBattery.TriggerEvent();
        disableSwitch.TriggerEvent();
    }

    private void OnEnable()
    {
        doorFirstClick.AddListener(HandleDoorFirstClick);
        switchOn.AddListener(HandleSwitchOn);
        powerOn.AddListener(HandlePowerOn);
    }

    private void OnDisable()
    {
        doorFirstClick.RemoveListener(HandleDoorFirstClick);
        switchOn.RemoveListener(HandleSwitchOn);
        powerOn.AddListener(HandlePowerOn);
    }

    // 1. User clicks the door button, trigger the power outage, enable power switch
    private void HandleDoorFirstClick()
    {
        StartCoroutine(TriggerPowerOutage());
        enableSwitch.TriggerEvent();
    }

    IEnumerator TriggerPowerOutage()
    {
        yield return new WaitForSeconds(1);
        powerOutageEvent.TriggerEvent();
        playerThoughts.text = "Oh no the power seems to have gone out. Maybe I need to change the power source.";
    }

    // 2. Once user switches, disable switch, enable battery
    private void HandleSwitchOn()
    {
        disableSwitch.TriggerEvent();
        enableBattery.TriggerEvent();
        playerThoughts.text = "The LED seems to indicate there is still no power. I need to find and insert the battery.";
        _clickedSwitch = true;
    }

    // 3. User can open the door with battery inserted
    private void HandlePowerOn()
    {
        if (_clickedSwitch)
        {
            playerThoughts.text = "The power seems to have come back on, I can now open the door.";
        }
    }
}
