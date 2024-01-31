using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTimeExtender : MonoBehaviour, IInteractable
{
    public TimeLoopController timeloopController;
    public string DisplayText { get
        {
            if (PickUpHoldScript.heldItemIdentifier == null)
                return "No Item held";
            if (requiredHeldItem == PickUpHoldScript.heldItemIdentifier)
                return "Fix time machine";
            return "Wrong item held";
        }
    }
    public HoldableItemScriptableObject requiredHeldItem;
    public PickUpHoldScript holdScript;

    public void Interact()
    {
        if (requiredHeldItem == PickUpHoldScript.heldItemIdentifier)
        {
            timeloopController.SetTime(timeloopController.GetTime() + 10.0f);
            timeloopController.ResetScene();
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
