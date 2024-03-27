using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IInteractable, IRayHoverable
{
    // public GameEvent controlPanelInteracted;
    [Header("Triggers")]
    public GameEvent LoginUIOn;

    public HintData controlPanelHints;
    public HintData passwordDatabaseHints;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        // controlPanelInteracted.TriggerEvent();
        LoginUIOn.TriggerEvent();   
        controlPanelHints.Unlock();
        passwordDatabaseHints.Unlock();
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}
