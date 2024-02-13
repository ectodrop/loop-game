using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTest : MonoBehaviour, IInteractable, IRayHoverable
{
    [SerializeField] private UnityEvent onInteract;

    private bool canInteract = true;


    public void OnHoverEnter()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.white);
    }

    public void OnHoverExit()
    {
        GetComponent<MeshRenderer>().materials.Last().SetColor("_Color", Color.black);
    }

    public void Interact()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
        onInteract.Invoke();
        canInteract = false;
    }

    public bool CanInteract()
    {
        return canInteract;
    }
}
