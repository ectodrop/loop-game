using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PC;

public class StartButton : MonoBehaviour, IInteractable, ILabel
{
    public string BootPCText;

    public string GetLabel()
    {
        return _canInteract ? BootPCText : "";
    }
    public bool CanInteract()
    {
        return _canInteract;
    }
    public void Interact() 
    {
        if (PCStatus == Status.Off)
        {
            Boot();
        }
    }
}
