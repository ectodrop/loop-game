using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabLightsController : MonoBehaviour
{
    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;

    private readonly int _defaultIntensity = 50;

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
        foreach (var light in GetComponentsInChildren<Light>())
        {
            light.intensity = _defaultIntensity;
        }
    }

    private void HandlePowerOff()
    {
        foreach (var light in GetComponentsInChildren<Light>())
        {
            light.intensity = 10;
        }
    }
}