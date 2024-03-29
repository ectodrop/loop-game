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

    public GameObject doorSwitch;
    
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
    public GameEventVector3 lookAtEvent;

    [Header("Listening To")]
    public GameEvent switchOn;
    public GameEvent batteryInserted;
    public GameEvent powerOff;
    public GameEvent doorOpened;

    private DialogueController _dialogueController;

    // Is the door opened already or not
    private bool _doorOpened = false;
    public void Start()
    {
        // Disable Battery and Switch
        lookAtEvent.TriggerEvent(doorSwitch.transform.position);
        disableBattery.TriggerEvent();
        disableSwitch.TriggerEvent();
        _dialogueController = FindObjectOfType<DialogueController>();
        _dialogueController.StartDialogue(playerThoughts, DialogueOptions.NO_INPUT | DialogueOptions.ALLOW_MOVEMENT);
    }

    private void OnEnable()
    {
        doorFirstClick.AddListener(HandleDoorFirstClick);
        switchOn.AddListener(HandleSwitchOn);
        batteryInserted.AddListener(HandleBatteryInserted);
        powerOff.AddListener(HandlePowerOff);
        doorOpened.AddListener(HandleDoorOpened);
    }

    private void OnDisable()
    {
        doorFirstClick.RemoveListener(HandleDoorFirstClick);
        switchOn.RemoveListener(HandleSwitchOn);
        batteryInserted.RemoveListener(HandleBatteryInserted);
        powerOff.RemoveListener(HandlePowerOff);
        doorOpened.RemoveListener(HandleDoorOpened);
    }

    private void HandleDoorOpened()
    {
        _doorOpened = true;
    }

    // 1. User clicks the door button, trigger the power outage, enable power switch
    private void HandleDoorFirstClick()
    {
        StartCoroutine(TryOpenDoor());
        enableBattery.TriggerEvent();
    }

    IEnumerator TryOpenDoor()
    {
        yield return new WaitForSeconds(1);
        _dialogueController.ProgressDialogue(true);
    }


    // 2. User can open the door with battery inserted
    private void HandleBatteryInserted()
    {
        enableSwitch.TriggerEvent();
        _dialogueController.ProgressDialogue(true);
    }
    
    // 3. Once user switches, disable switch, enable battery
    private void HandleSwitchOn()
    {
        _dialogueController.ProgressDialogue(true);
        _clickedSwitch = true;
    }

    // 4. User did not exit the tutorial fast enough prompt them to loop reset
    private void HandlePowerOff()
    {
        if (!_doorOpened)
        {
            _dialogueController.ProgressDialogue(true);
        }
    }
}
