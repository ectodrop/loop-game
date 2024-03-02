using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameControls", fileName = "NewGameControls")]
public class GameControls : ScriptableObject
{
    public GameControlsAsset Asset { get; private set; }

    private void OnEnable()
    {
        Asset = new GameControlsAsset();
    }
}
