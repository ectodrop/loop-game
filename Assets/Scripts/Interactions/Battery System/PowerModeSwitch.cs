using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerModeSwitch : MonoBehaviour, IInteractable, IRayHoverable, ILabel
{
    public string EmergencyOnText;
    public string EmergencyOffText;

    public GameObject holder;
    private BatteryHolder _holderScript;
    public GameEvent switchOn;
    public GameEvent switchOff;

    public HintData emergencyPowerHintData;

    private bool _canInteract = true;
    private bool _emergencyPower = false;

    // Manually Enable and disable switch
    public GameEvent enableSwitch;
    public GameEvent disableSwitch;

    // Offsets for rotation
    // private Vector3 offPos;
    // private Vector3 onPos = new Vector3(-0.413f, -0.22f, 0.1473789f);

    private void Start()
    {
        _holderScript = holder.GetComponent<BatteryHolder>();
        // offPos = gameObject.transform.localPosition;
    }

    private void OnEnable()
    {
        enableSwitch.AddListener(HandleEnableSwitch);
        disableSwitch.AddListener(HandleDisableSwitch);
    }

    private void OnDisable()
    {
        enableSwitch.RemoveListener(HandleEnableSwitch);
        disableSwitch.RemoveListener(HandleDisableSwitch);
    }

    private void HandleEnableSwitch()
    {
        _canInteract = true;
    }

    private void HandleDisableSwitch()
    {
        _canInteract = false;
    }

    public string GetLabel()
    {
        if (_canInteract)
        {
            return _emergencyPower ? EmergencyOnText : EmergencyOffText;
        }

        return "";
    }

    public void OnHoverEnter()
    {
        // GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
    }

    public void OnHoverExit()
    {
        // GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.black);
    }

    public void Interact()
    {
        // Switch on
        if (!_emergencyPower)
        {
            TurnOn();
            emergencyPowerHintData.Unlock();
        }
        // Switch off
        else
        {
            TurnOff();
        }
    }

    private void TurnOn()
    {

        // If the holder already has the battery start the battery drain
        if (_holderScript.HasBattery() && !_holderScript.IsBatteryEmpty())
        {
            _holderScript.StartBatteryDrain();
        }

        _emergencyPower = true;
        switchOn.TriggerEvent();
        gameObject.transform.transform.eulerAngles += new Vector3(-90, 0, 0);
        // gameObject.transform.localPosition = onPos;
    }

    private void TurnOff()
    {
        // GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        _emergencyPower = false;
        switchOff.TriggerEvent();
        // Stop draining the battery if not already empty to prevent uneeded triggers
        if (!_holderScript.IsBatteryEmpty())
        {
            _holderScript.StopDrain();
        }

        gameObject.transform.transform.eulerAngles += new Vector3(90, 0, 0);
        // gameObject.transform.localPosition = offPos;
    }

    public bool CanInteract()
    {
        return _canInteract;
    }

    public bool UsingEmergencyPower()
    {
        return _emergencyPower;
    }
}