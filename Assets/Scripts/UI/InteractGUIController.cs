using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractGUIController : MonoBehaviour
{
    public TextMeshProUGUI interactableLabel;
    public GameEventString onHoverReadableObject;
    public GameEvent HUDEnableEvent, HUDDisableEvent;


    public void OnEnable()
    {
        onHoverReadableObject.AddListener(SetText);
        HUDEnableEvent.AddListener(Show);
        HUDDisableEvent.AddListener(Hide);
    }

    public void OnDisable()
    {
        onHoverReadableObject.RemoveListener(SetText);
        HUDEnableEvent.RemoveListener(Show);
        HUDDisableEvent.RemoveListener(Hide);
    }

    private void Hide()
    {
        interactableLabel.gameObject.SetActive(false);
    }

    private void Show()
    {
        interactableLabel.gameObject.SetActive(true);
    }
    
    private void SetText(String label)
    {
        interactableLabel.text = label;
    }

}
