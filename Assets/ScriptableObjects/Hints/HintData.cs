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
    public bool defaultUnlocked; // has the player unlocked this hint
    public bool defaultShown; // has the hint been pointed out to the player
    public bool defaultRead; // has the hint been "read" (the player interacted with the hint)

    [SerializeField] private bool _unlocked;
    [SerializeField] private bool _shown;
    [SerializeField] private bool _read;

    private void OnEnable()
    {
        _unlocked = defaultUnlocked;
        _shown = defaultShown;
        _read = defaultRead;
    }

    public void Unlock()
    {
        _unlocked = true;
    }

    public void SetShown()
    {
        _shown = true;
    }

    public void SetRead()
    {
        _read = true;
    }

    public bool WasShown()
    {
        return _shown;
    }

    public bool IsUnlocked()
    {
        return _unlocked;
    }

    public bool IsRead()
    {
        return _read;
    }

    public bool HasDialogue()
    {
        return hintDialogue != null;
    }
}
