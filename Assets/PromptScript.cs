using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PromptScript : MonoBehaviour
{
    public GameControls gameControls;
    public Color dialogueAvailable;
    public Color dialogueUnAvailable;
    
    public Image[] hintImages;
    public GameObject hint;
    public GameObject circle;
    public GameObject interactText;
    public GameObject holdInteractionBar;
    public TextMeshProUGUI label;
    public Animator hintBubbleAnimator;
    public Animator notificationRingAnimator;

    private HintData _hintData;

    private bool _enabled = false;
    private IEnumerator _fillRoutine;

    private void OnEnable()
    {
        gameControls.Wrapper.Player.HoldInteract.started += OnHoldStart;
        gameControls.Wrapper.Player.HoldInteract.performed += OnHoldEnd;
        gameControls.Wrapper.Player.HoldInteract.canceled += OnHoldEnd;
    }

    private void OnDisable()
    {
        gameControls.Wrapper.Player.HoldInteract.started -= OnHoldStart;
        gameControls.Wrapper.Player.HoldInteract.performed -= OnHoldEnd;
        gameControls.Wrapper.Player.HoldInteract.canceled -= OnHoldEnd;
    }

    private void OnHoldStart(InputAction.CallbackContext context)
    {
        if (!_enabled)
            return;
        var holdInteract = (HoldInteraction)context.interaction;
        _fillRoutine = FillHoldBar(holdInteract.duration > 0.0f ? holdInteract.duration : InputSystem.settings.defaultHoldTime);
        StartCoroutine(_fillRoutine);
    }
    
    private void OnHoldEnd(InputAction.CallbackContext context)
    {
        if (!_enabled)
            return;
        holdInteractionBar.transform.localScale = new Vector3(1, 0, 1);
        StopCoroutine(_fillRoutine);
    }

    private IEnumerator FillHoldBar(float duration)
    {
        float startTime = Time.realtimeSinceStartup;
        Debug.Log(duration);
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float t = Time.realtimeSinceStartup - startTime;
            holdInteractionBar.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, t / duration), 1); 
                
            yield return null;
        }
    }

    private void SetImageColors(Color color)
    {
        for (int i = 0; i < hintImages.Length; i++)
        {
            hintImages[i].color = color;
        }
    }
    
    public void SetHintData(HintData hintData)
    {
        _enabled = true;
        _hintData = hintData;
        if (hintData.hintDialogue != null)
        {
            if (hintData.IsRead())
            {
                SetImageColors(dialogueUnAvailable);
            }
            else
            {
                SetImageColors(dialogueAvailable);
            }
        }
        else
        {
            interactText.SetActive(false);
            SetImageColors(dialogueUnAvailable);
        }
    }

    public void MarkAsRead()
    {
        if (_enabled)
        {
            _hintData.SetRead();
            SetImageColors(dialogueUnAvailable);
        }
    }

    public void PlayNotification()
    {
        notificationRingAnimator.gameObject.SetActive(true);
        notificationRingAnimator.SetTrigger("TriggerRing");
    }
    
    public void ShowHint()
    {
        hint.SetActive(true);
        hintBubbleAnimator.SetTrigger("ShowHint");
        hintBubbleAnimator.ResetTrigger("HideHint");
        if (_enabled)
        {
            if (_hintData.hintDialogue != null)
            {
                interactText.SetActive(true);
            }
            label.text = _hintData.Label;
        }
        else
        {
            interactText.SetActive(false);
            label.text = "???";
        }
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }
}
