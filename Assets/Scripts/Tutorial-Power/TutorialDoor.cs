using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IInteractable, IRayHoverable, ILabel
{
    public GameObject door;
    public int maxDoorHeight;
    public GameEvent doorFirstClick;
    public ScheduleEvent powerOutageEvent;

    private bool canInteract = true;
    private bool _firstPress = false;
    private string OffText = "Turn On (E)";

    // Set power flow status
    public GameEvent powerOn;
    public GameEvent powerOff;
    private bool _hasPower = true;

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
            _firstPress = true;
            _hasPower = false;
            SetGreen();
            doorFirstClick.TriggerEvent();
            StartCoroutine(ResetButton());
        }
        // Power && Not first press
        else if (_firstPress && _hasPower)
        {
            SetGreen();
            StartCoroutine(AnimateDoor());
        }
    }

    IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(1);
        SetRed();
    }

    IEnumerator AnimateDoor()
    {
        float height = door.transform.position.y;
        while (height < maxDoorHeight)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 newPosition = door.transform.position + Vector3.up * 0.01f;
            door.transform.position = newPosition;
            height = door.transform.position.y;
        }
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