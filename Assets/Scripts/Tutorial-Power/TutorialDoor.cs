using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IInteractable, IRayHoverable, ILabel
{
    public DoorScript door;
    public int maxDoorHeight;
    public GameEvent doorFirstClick;
    public ScheduleEvent powerOutageEvent;
    public SoundEffect garageDoorOpen;
    public SoundEffect buttonClick;

    private bool canInteract = true;
    private bool _firstPress = false;
    private string OffText = "Turn On (E)";

    // Set power flow status
    public GameEvent powerOn;
    public GameEvent powerOff;
    private bool _hasPower = true;

    // Event to indicate the door is opening
    public GameEvent doorOpened;

    private void OnEnable()
    {
        powerOutageEvent.AddListener(HandlePowerOutage);
        powerOn.AddListener(HandlePowerOn);
        powerOff.AddListener(HandlerPowerOff);
    }

    private void OnDisable()
    {
        powerOutageEvent.RemoveListener(HandlePowerOutage);
        powerOn.RemoveListener(HandlePowerOn);
        powerOff.RemoveListener(HandlerPowerOff);
    }

    private void HandlePowerOutage()
    {
        SetRed();
    }

    private void HandlePowerOn()
    {
        _hasPower = true;
    }

    private void HandlerPowerOff()
    {
        _hasPower = false;
    }

    private void Start()
    {
        SetRed();
    }

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
        // First click
        if (!_firstPress)
        {
            buttonClick.Play();
            _firstPress = true;
            _hasPower = false;
            SetGreen();
            doorFirstClick.TriggerEvent();
            StartCoroutine(ResetButton());
        }
        // Power && Not first press
        else if (_firstPress && _hasPower)
        {
            buttonClick.Play();
            SetGreen();
            door.OpenDoor();
            doorOpened.TriggerEvent();
        }
    }

    IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(1);
        SetRed();
    }

    private void SetRed()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.red);
    }

    private void SetGreen()
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public string GetLabel()
    {
        return _hasPower ? OffText : "";
    }
}
