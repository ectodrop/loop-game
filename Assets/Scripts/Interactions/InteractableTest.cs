using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] private string hintText = "Press E";
    [SerializeField] private UnityEvent onInteract;
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
