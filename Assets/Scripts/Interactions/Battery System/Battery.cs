using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour, IInteractable, IRayHoverable, ILabel
{
    public string pickupText;
    // Manually Enable and disable pickup
    public GameEvent enableBattery;
    public GameEvent disableBattery;

    // Manually Enable and Disable drain feature
    public GameEvent enableDrain;
    public GameEvent disableDrain;

    private bool _canDrain = true;
    
    private int _batteryLevel = 100;


    private bool _canInteract = true;

    private void OnEnable()
    {
        enableBattery.AddListener(HandleEnableBattery);
        disableBattery.AddListener(HandleDisableBattery);
        enableDrain.AddListener(HandleEnableDrain);
        disableDrain.AddListener(HandleDisableDrain);
    }

    private void OnDisable()
    {
        enableBattery.RemoveListener(HandleEnableBattery);
        disableBattery.RemoveListener(HandleDisableBattery);
        enableDrain.RemoveListener(HandleEnableDrain);
        disableDrain.RemoveListener(HandleDisableDrain);
    }

    private void HandleEnableBattery()
    {
        _canInteract = true;
        gameObject.transform.gameObject.tag = "canPickUp";
    }

    private void HandleDisableBattery()
    {
        _canInteract = false;
        gameObject.transform.gameObject.tag = "Untagged";
    }

    private void HandleEnableDrain()
    {
        _canDrain = true;
    }

    private void HandleDisableDrain()
    {
        _canDrain = false;
    }

    public void OnHoverEnter()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
    }

    public void OnHoverExit()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.yellow);
    }

    public void Interact()
    {
        _canInteract = false;
    }

    public bool CanInteract()
    {
        return _canInteract;
    }

    public int GetBatteryLevel()
    {
        return _batteryLevel;
    }

    public void DecreaseBattery(int amount)
    {
        if (_canDrain)
        {
            _batteryLevel -= amount;
        }
    }

    public string GetLabel()
    {
        if (_canInteract)
        {
            return pickupText;
        }
        return "";
    }
}
