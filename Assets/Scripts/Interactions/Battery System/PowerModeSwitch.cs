using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerModeSwitch : MonoBehaviour, IInteractable, IRayHoverable
{
    [SerializeField] private string hintText = "Press E";
    [SerializeField] private UnityEvent onInteract;

    public GameObject holder;
    private BatteryHolder _holderScript;

    public string DisplayText
    {
        get => hintText;
    }

    private bool canInteract = true;
    private bool _emergencyPower = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
        _holderScript = holder.GetComponent<BatteryHolder>();
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
            if (_holderScript.HasBattery())
            {
                _holderScript.StartBatteryDrain();
            }
            
            _emergencyPower = true;
        }
        // Switch off
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
            _emergencyPower = false;
            
            // Stop draining the battery
            _holderScript.StopDrain();
        }

        onInteract.Invoke();
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