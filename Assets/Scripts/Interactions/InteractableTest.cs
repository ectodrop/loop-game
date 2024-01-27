using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : InteractableObject
{
    private bool canInteract = true;


    public override void Interact()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        onInteract.Invoke();
        canInteract = false;
    }

    public override bool CanInteract()
    {
        return canInteract;
    }
}
