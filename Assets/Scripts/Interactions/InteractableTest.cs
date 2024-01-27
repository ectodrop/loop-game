using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] protected string hintText = "Press E";
    [SerializeField] protected UnityEvent onInteract;
    public string DisplayText { get => hintText; }

    private bool canInteract = true;


    public void Interact()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        onInteract.Invoke();
        canInteract = false;
    }

    public bool CanInteract()
    {
        return canInteract;
    }
}
