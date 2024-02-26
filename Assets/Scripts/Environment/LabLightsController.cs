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

    private LightmapData[] darkLightMap, brightLightmap;
    
    [Header("Listening To")]
    public GameEvent powerOn;
    public GameEvent powerOff;

    // private readonly int _defaultIntensity = 50;

    private void Start()
    {
        List<LightmapData> dlightMap = new List<LightmapData>();
        for (int i = 0; i < darkLightmapColor.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];
            
            dlightMap.Add(lmdata);
        }

        darkLightMap = dlightMap.ToArray();
        
        List<LightmapData> blightMap = new List<LightmapData>();
        for (int i = 0; i < brightLightmapColor.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];
            
            blightMap.Add(lmdata);
        }

        brightLightmap = blightMap.ToArray();
    }

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
        LightmapSettings.lightmaps = brightLightmap;
        foreach (var light in GetComponentsInChildren<MeshRenderer>())
        {
            light.material = lightOn;
        }
    }

    private void HandlePowerOff()
    {
        LightmapSettings.lightmaps = darkLightMap;
        foreach (var light in GetComponentsInChildren<MeshRenderer>())
        {
            light.material = lightOff;
        }
    }
}