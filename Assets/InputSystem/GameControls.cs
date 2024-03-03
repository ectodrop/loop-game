using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameControls", fileName = "NewGameControls")]
public class GameControls : ScriptableObject
{
    public GameControlsAsset Wrapper { get; private set; }

    private void OnEnable()
    {
        Wrapper = new GameControlsAsset();
        Wrapper.Enable();
    }

    private void OnDisable()
    {
        Wrapper.Disable();
    }
}
