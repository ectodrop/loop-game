using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCoreHandler : MonoBehaviour, IInteractable
{
    [Header("Triggers")]
    public GameEventInt timeExtensionEvent;
    public void Interact()
    {
        timeExtensionEvent.TriggerEvent(10);
    }

    public bool CanInteract()
    {
        return true;
    }
}
