using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battery : MonoBehaviour, IInteractable, IRayHoverable
{
    [SerializeField] private string hintText = "Press E";
    [SerializeField] private UnityEvent onInteract;

    private int _batteryLevel = 100;

    public string DisplayText
    {
        get => hintText;
    }

    private bool canInteract = true;


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
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
        onInteract.Invoke();
        canInteract = false;
    }

    public bool CanInteract()
    {
        return canInteract;
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