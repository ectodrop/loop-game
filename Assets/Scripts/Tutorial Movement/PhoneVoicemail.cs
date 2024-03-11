using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneVoicemail : MonoBehaviour, IInteractable, ILabel
{
    public DialogueNode readVoiceMail;
    private DialogueController _dialogueControlller;

    private void Start()
    {
        _dialogueControlller = FindObjectOfType<DialogueController>();
    }

    public void Interact()
    {
        _dialogueControlller.StartDialogue(readVoiceMail, DialogueOptions.NONE);
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetLabel()
    {
        return "Read Voicemail [E]";
    }
}
