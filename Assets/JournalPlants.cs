using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalPlants : MonoBehaviour, ILabel, IInteractable
{
    public DialogueNode openJournalDialogue;
    public DialogueNode entry1Dialogue;
    public DialogueNode entry2Dialogue;
    public DialogueNode entry3Dialogue;

    private DialogueController _dialogueController;

    private void Start()
    {
        _dialogueController = FindObjectOfType<DialogueController>();
    }

    public void Interact()
    {
        _dialogueController.StartDialogue(openJournalDialogue, choiceCallback: ReadEntry);
    }

    public void ReadEntry(string entry)
    {
        switch (entry)
        {
            case "Entry 1":
                _dialogueController.StartDialogue(entry1Dialogue);
                break;
            case "Entry 2":
                _dialogueController.StartDialogue(entry2Dialogue);
                break;
            case "Entry 3":
                _dialogueController.StartDialogue(entry3Dialogue);
                break;
            default:
                break;
        }
    }
    
    public bool CanInteract()
    {
        return true;
    }


    public string GetLabel()
    {
        return "Read Journal [E]";
    }
}
