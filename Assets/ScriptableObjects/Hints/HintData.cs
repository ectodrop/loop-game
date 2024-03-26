using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "HintData", fileName = "NewHintData")]
public class HintData : ScriptableObject
{
    public string Label;

    public DialogueNode hintDialogue;
    // public so we can see them in the inspector
    public bool defaultUnlocked;
    public bool defaultShown;
    [SerializeField] private bool _unlocked;
    [SerializeField] private bool _shown;

    private void OnEnable()
    {
        _unlocked = defaultUnlocked;
        _shown = defaultShown;
    }

    public void Unlock()
    {
        _unlocked = true;
    }

    public void SetShown()
    {
        _shown = true;
    }

    public bool WasShown()
    {
        return _shown;
    }

    public bool IsUnlocked()
    {
        return _unlocked;
    }
}
