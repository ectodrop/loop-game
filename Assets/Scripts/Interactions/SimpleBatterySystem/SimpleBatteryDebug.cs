using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBatteryDebug : MonoBehaviour
{
    public GameEvent powerOn;
    public GameEvent powerOff;

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
        Debug.Log("Power ON triggered.");
    }

    private void HandlePowerOff()
    {
        Debug.Log("Power OFF triggered.");
    }
}
