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
    public DialogueNode playerThoughts;
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

    private DialogueController _dialogueController;

    public void Start()
    {
        // Disable Battery and Switch
        disableBattery.TriggerEvent();
        disableSwitch.TriggerEvent();
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.StartDialogue(playerThoughts, DialogueOptions.NO_INPUT | DialogueOptions.ALLOW_MOVEMENT);
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
        _dialogueController.ProgressDialogue();
    }

    // 2. Once user switches, disable switch, enable battery
    private void HandleSwitchOn()
    {
        disableSwitch.TriggerEvent();
        enableBattery.TriggerEvent();
        _dialogueController.ProgressDialogue();
        _clickedSwitch = true;
    }

    // 3. User can open the door with battery inserted
    private void HandlePowerOn()
    {
        if (_clickedSwitch)
        {
            _dialogueController.ProgressDialogue();
        }
    }
}
