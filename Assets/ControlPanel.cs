using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IInteractable
{
    public GameEvent controlPanelInteracted;

    public string DisplayText => "Press E to interact with Control Panel";

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        controlPanelInteracted.TriggerEvent();
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
