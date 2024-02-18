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


    private bool canInteract = true;
    private bool _emergencyPower = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        _holderScript = holder.GetComponent<BatteryHolder>();
    }

    public string GetLabel()
    {
        return _emergencyPower ? EmergencyOnText : EmergencyOffText;
    }

    public void OnHoverEnter()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
    }

    public void OnHoverExit()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.black);
    }

    public void Interact()
    {
        // Switch on
        if (!_emergencyPower)
        {
            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);

            // If the holder already has the battery start the battery drain
            if (_holderScript.HasBattery() && !_holderScript.IsBatteryEmpty())
            {
                _holderScript.StartBatteryDrain();
            }

            _emergencyPower = true;
            switchOn.TriggerEvent();
        }
        // Switch off
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            _emergencyPower = false;
            switchOff.TriggerEvent();
            // Stop draining the battery if not already empty to prevent uneeded triggers
            if (!_holderScript.IsBatteryEmpty())
            {
                _holderScript.StopDrain();
            }
        }
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public bool UsingEmergencyPower()
    {
        return _emergencyPower;
    }
}