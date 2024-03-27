using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LabLightsController : MonoBehaviour
{
    public LightMapAsset brightLightMapAsset;
    public LightMapAsset darkLightMapAsset;
    public Material lightOn;
    public Material lightOff;

    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent LightFlicker;
    private const float _flickerDuration = 0.3f;
    private float _flickerCountDown;
    private bool _flickering = false;

    private void Start()
    {
        _flickerCountDown = _flickerDuration;
    }

    private void OnEnable()
    {
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlePowerOff);
        LightFlicker.AddListener(Flicker);
    }

    private void OnDisable()
    {
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlePowerOff);
        LightFlicker.RemoveListener(Flicker);
    }

    private void HandlePowerOn()
    {
        LightmapSettings.lightmaps = brightLightMapAsset.GetLightMapData();
        LightmapSettings.lightProbes.bakedProbes = brightLightMapAsset.lightProbeData;
        foreach (var light in GetComponentsInChildren<MeshRenderer>())
        {
            light.material = lightOn;
        }
    }

    private void HandlePowerOff()
    {
        LightmapSettings.lightmaps = darkLightMapAsset.GetLightMapData();
        LightmapSettings.lightProbes.bakedProbes = darkLightMapAsset.lightProbeData;
        foreach (var light in GetComponentsInChildren<MeshRenderer>())
        {
            light.material = lightOff;
        }
    }
    
    private void Update()
    {
        if (_flickering)
        {
            _flickerCountDown -= Time.deltaTime;
            if (_flickerCountDown <= 0f)
            {
                powerOn.TriggerEvent();
                _flickering = false;
            }
        }
        
        
    }
    void Flicker()
    {
        _flickering = true;
        powerOff.TriggerEvent();
    }
}