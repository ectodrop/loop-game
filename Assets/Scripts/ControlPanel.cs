using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IInteractable, IRayHoverable
{
    // public GameEvent controlPanelInteracted;
    [Header("Triggers")]
    public GameEvent LoginUIOn;


    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        // controlPanelInteracted.TriggerEvent();
        LoginUIOn.TriggerEvent();   
        
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}
