using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueNode", fileName = "NewDialogueNode")]
public class DialogueNode : ScriptableObject
{
    public Dialogue[] sentences;
}

