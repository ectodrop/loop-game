using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LabLightsController : MonoBehaviour
{
    public Texture2D[] darkLightmapDir, darkLightmapColor;
    public Texture2D[] brightLightmapDir, brightLightmapColor;
    public Material lightOn;
    public Material lightOff;

    private LightmapData[] _darkLightMap, _brightLightmap;
    
    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;
    public GameEvent LightFlicker;
    private const float _flickerDuration = 1.0f;
    private float _flickerCountDown;
    private bool _flickering = false;

    private void Start()
    {
        _flickerCountDown = _flickerDuration;
        List<LightmapData> dlightMap = new List<LightmapData>();
        for (int i = 0; i < darkLightmapColor.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];
            
            dlightMap.Add(lmdata);
        }

        _darkLightMap = dlightMap.ToArray();
        
        List<LightmapData> blightMap = new List<LightmapData>();
        for (int i = 0; i < brightLightmapColor.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];
            
            blightMap.Add(lmdata);
        }

        _brightLightmap = blightMap.ToArray();
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
        LightmapSettings.lightmaps = _brightLightmap;
        foreach (var light in GetComponentsInChildren<MeshRenderer>())
        {
            light.material = lightOn;
        }
    }

    private void HandlePowerOff()
    {
        LightmapSettings.lightmaps = _darkLightMap;
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
            }
        }
        
        
    }
    void Flicker()
    {
        _flickering = true;
        powerOff.TriggerEvent();
    }
}