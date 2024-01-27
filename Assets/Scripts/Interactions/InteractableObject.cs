using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected string hintText = "Press E";
    [SerializeField] protected UnityEvent onInteract;
    public string DisplayText { get => hintText; }

    public virtual void Interact()
    {
        onInteract.Invoke();
    }
    
    public virtual bool CanInteract()
    {
        return true;
    }
}
