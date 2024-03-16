using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WarningText : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public Color warningTextColor;
    [Header("Listening To")]
    public GameEventString setWarningTextEvent;
    public GameEvent flashWarningTextEvent;
    public GameEvent fadeWarningTextEvent;
    public GameEvent HUDEnableEvent;
    public GameEvent HUDDisableEvent;

    public float numFlashes;

    public float fadeDuration;


    private IEnumerator _flashRoutine;
    private IEnumerator _fadeRoutine;
    private void OnEnable()
    {
        setWarningTextEvent.AddListener(SetWarningText);
        flashWarningTextEvent.AddListener(FlashWarningText);
        fadeWarningTextEvent.AddListener(FadeWarningText);
        HUDDisableEvent.AddListener(HideWarningText);
        HUDEnableEvent.AddListener(ShowWarningText);
    }

    private void OnDisable()
    {
        setWarningTextEvent.RemoveListener(SetWarningText);
        flashWarningTextEvent.RemoveListener(FlashWarningText);
        fadeWarningTextEvent.RemoveListener(FadeWarningText);
        HUDDisableEvent.RemoveListener(HideWarningText);
        HUDEnableEvent.RemoveListener(ShowWarningText);
    }


    private void SetWarningText(string text)
    {
        ShowWarningText();
        warningText.text = text;
        warningText.color = warningTextColor;
    }

    private void FlashWarningText()
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);
        _flashRoutine = AnimateFlash();
        StartCoroutine(_flashRoutine);
    }

    private void FadeWarningText()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
        _fadeRoutine = AnimateFade();
        StartCoroutine(_fadeRoutine);
    }
    
    private IEnumerator AnimateFlash()
    {
        Color transparent = warningTextColor;
        transparent.a = 0;
        for (int i = 0; i < numFlashes; i++)
        {
            warningText.color = warningTextColor;
            yield return new WaitForSeconds(0.1f);
            warningText.color = transparent;
            yield return new WaitForSeconds(0.1f);
        }
        
        HideWarningText();
    }


    private IEnumerator AnimateFade()
    {
        Color transparent = warningTextColor;
        transparent.a = 0;
        float t = 0;
        while (t < fadeDuration)
        {
            warningText.color = Color.Lerp(warningTextColor, transparent, t/fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        
        HideWarningText();
    }
    
    private void ShowWarningText()
    {
        warningText.gameObject.SetActive(true);
    }

    private void HideWarningText()
    {
        warningText.gameObject.SetActive(false);
    }
}
