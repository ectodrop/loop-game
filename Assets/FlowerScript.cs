using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour, IInteractable, ILabel
{
    public void Interact()
    {
        // trigger win animation
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetLabel()
    {
        return "Collect Flower [E]";
    }
}
