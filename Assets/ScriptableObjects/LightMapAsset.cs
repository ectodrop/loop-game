using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LightMapAsset", fileName = "NewLightMapAsset")]
public class LightMapAsset : ScriptableObject
{
    public List<Texture2D> lightMapColors = new List<Texture2D>();
    public List<Texture2D> lightMapDirs = new List<Texture2D>();


    private LightmapData[] _lightmapDataCache;
    public LightmapData[] GetLightMapData()
    {
        if (lightMapColors.Count != lightMapDirs.Count)
        {
            Debug.LogWarning("Mismatched light maps lists");
            return null;
        }
        if (_lightmapDataCache != null)
        {
            return _lightmapDataCache;
        }
        var maps = new List<LightmapData>();
        
        for (int i = 0; i < lightMapColors.Count; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = lightMapDirs[i];
            lmdata.lightmapColor = lightMapColors[i];
            maps.Add(lmdata);
        }

        _lightmapDataCache = maps.ToArray();
        return _lightmapDataCache;
    }
    
    public void Clear()
    {
        lightMapColors.Clear();
        lightMapDirs.Clear();
        _lightmapDataCache = null;
    }
}
