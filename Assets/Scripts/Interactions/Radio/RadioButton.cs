using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadioButton : MonoBehaviour, IInteractable, IRayHoverable, ILabel
{
    public bool debug;
    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;

    [Header("Triggers")]
    public GameEvent radioButtonClick;

    private bool _canInteract = true;
    private bool _hasPower = false;

    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
    }
    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
    }
    private void HandlePowerOn()
    {
        _hasPower = true;
    }

    private void HandlePowerOff()
    {
        _hasPower = false;
    }

    public void Interact()
    {
        Debug.Log("here");
        if (_hasPower || debug)
        {
            radioButtonClick.TriggerEvent();
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetLabel()
    {
        return _hasPower ? "Switch Station (E)" : "Radio not Charged";
    }

    public void OnHoverEnter()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
    }

    public void OnHoverExit()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.black);
    }
}
