using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "HintData", fileName = "NewHintData")]
public class HintData : ScriptableObject
{
    public string Label;
    [NonSerialized]
    public List<string> Hints;

    private void OnEnable()
    {
        Hints = new List<string>();
    }


    public void AddHint(string hint)
    {
        Hints.Add(hint);
    }
}
