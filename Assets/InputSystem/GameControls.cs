using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameControls", fileName = "NewGameControls")]
public class GameControls : ScriptableObject
{
    private GameControlsAsset _wrapper;
    public GameControlsAsset Wrapper
    {
        get
        {
            if (_wrapper == null) _wrapper = new GameControlsAsset();
            return _wrapper;
        }
    }

    private void OnEnable()
    {
        Wrapper.Enable();
    }

    private void OnDisable()
    {
        Wrapper.Disable();
    }
}
