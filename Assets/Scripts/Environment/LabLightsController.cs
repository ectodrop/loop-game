using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabLightsController : MonoBehaviour
{
    public Light light;
    public GameEvent powerOn;
    public GameEvent powerOff;

    private readonly int _defaultIntensity = 300;

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
        light.intensity = _defaultIntensity;
    }

    private void HandlePowerOff()
    {
        light.intensity = 0;
    }
}