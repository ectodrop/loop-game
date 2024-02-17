using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IInteractable, IRayHoverable
{
    // public GameEvent controlPanelInteracted;
    public GameEvent LoginUIOn;


    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        // controlPanelInteracted.TriggerEvent();
        LoginUIOn.TriggerEvent();   
        
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}
