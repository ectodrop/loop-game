using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCoreHandler : MonoBehaviour, IInteractable
{
    [Header("Triggers")]
    public GameEventFloat timeExtensionEvent;
    public void Interact()
    {
        timeExtensionEvent.TriggerEvent(10.0f);
    }

    public bool CanInteract()
    {
        return true;
    }
}
