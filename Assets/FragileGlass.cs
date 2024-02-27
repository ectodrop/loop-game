using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileGlass : MonoBehaviour, IInteractable, ILabel
{
    public PowerGenerator powerGenerator;
    public SoundEffect glassBreakSFX;
    public HoldableItemScriptableObject hammer;

    public string GetLabel()
    {
        if (powerGenerator.drained && IsHoldingHammer())
            return "Break Glass (E)";
        if (!powerGenerator.drained && IsHoldingHammer())
            return "Can't break when filled with liquid";
        if (!powerGenerator.drained && !IsHoldingHammer())
            return "Can't break when filled with liquid";
        return "Need a hammer to break";
    }
    
    private bool IsHoldingHammer()
    {
        return PickUpHoldScript.heldItemIdentifier == hammer;
    }
    
    public void Interact()
    {
        Debug.Log("Here");
        if (powerGenerator.drained && IsHoldingHammer())
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
