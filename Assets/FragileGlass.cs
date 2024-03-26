using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileGlass : MonoBehaviour, IInteractable, ILabel
{
    public PowerGenerator powerGenerator;
    public SoundEffect glassBreakSFX;
    public HoldableItemScriptableObject hammer;
    public HoldableItemScriptableObject battery;
    public DialogueNode hammerlessFragileGlassDialogue;
    public DialogueNode undrainedHammerFragileGlassDialogue;
    public DialogueNode batteryFragileGlassDialogue;

    private DialogueController _dialogueController;

    private void Start()
    {
        _dialogueController = FindObjectOfType<DialogueController>();
    }

    public string GetLabel()
    {
        if (powerGenerator.drained && IsHoldingHammer())
            return "Smash Glass!! (E)";
        
        if (!powerGenerator.drained && IsHoldingHammer())
            return "Break Glass with Hammer? (E)";
        if (IsHoldingBattery())
            return "Break Glass with Battery? (E)";
        return "Break Glass? (E)";
    }
    
    private bool IsHoldingHammer()
    {
        return PickUpHoldScript.heldItemIdentifier == hammer;
    }

    private bool IsHoldingBattery()
    {
        return PickUpHoldScript.heldItemIdentifier == battery;
    }
    
    public void Interact()
    {
        if (IsHoldingBattery())
        {
            _dialogueController.StartDialogue(batteryFragileGlassDialogue, DialogueOptions.STOP_TIME);
        }
        else if (!IsHoldingHammer())
        {
            _dialogueController.StartDialogue(hammerlessFragileGlassDialogue, DialogueOptions.STOP_TIME);
        }
        else if (!powerGenerator.drained && IsHoldingHammer())
        {
            _dialogueController.StartDialogue(undrainedHammerFragileGlassDialogue, DialogueOptions.STOP_TIME);
        }
        else if (powerGenerator.drained && IsHoldingHammer())
        {
            glassBreakSFX.Play();
            gameObject.SetActive(false);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
