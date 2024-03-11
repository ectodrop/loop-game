using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneVoicemail : MonoBehaviour, IInteractable, ILabel
{
    public DialogueNode readVoiceMail;
    private DialogueController _dialogueControlller;

    public DialogueNode[] voicemails;

    private void Start()
    {
        _dialogueControlller = FindObjectOfType<DialogueController>();
    }
    
    public void Interact()
    {
        _dialogueControlller.StartDialogue(readVoiceMail, choiceCallback: PlayVoiceMails);
    }

    public void PlayVoiceMails(string voicemail)
    {
        switch (voicemail)
        {
            case "Voicemail 1":
                _dialogueControlller.StartDialogue(voicemails[0], DialogueOptions.INTERRUPTING);
                break;
        }
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
