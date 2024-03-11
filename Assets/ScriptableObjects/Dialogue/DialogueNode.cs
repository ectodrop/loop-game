using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueNode", fileName = "NewDialogueNode")]
public class DialogueNode : ScriptableObject
{
    public Dialogue[] sentences;

    public string[] choices;

    public bool HasChoices()
    {
        return choices != null && choices.Length > 0;
    }
}

