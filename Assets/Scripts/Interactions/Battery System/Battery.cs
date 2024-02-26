using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour, IInteractable, IRayHoverable
{
    // Manually Enable and disable pickup
    public GameEvent enableBattery;
    public GameEvent disableBattery;
    
    private int _batteryLevel = 100;


    private bool _canInteract = true;

    private void OnEnable()
    {
        enableBattery.AddListener(HandleEnableBattery);
        disableBattery.AddListener(HandleDisableBattery);
    }

    private void OnDisable()
    {
        enableBattery.RemoveListener(HandleEnableBattery);
        disableBattery.RemoveListener(HandleDisableBattery);
    }

    private void HandleEnableBattery()
    {
        _canInteract = true;
    }

    private void HandleDisableBattery()
    {
        _canInteract = false;
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
        _batteryLevel -= amount;
    }
}