using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptScript : MonoBehaviour
{
    public Color dialogueAvailable;
    public Color dialogueUnAvailable;
    
    public Image[] hintImages;
    public GameObject hint;
    public GameObject circle;
    public GameObject interactText;
    public TextMeshProUGUI label;
    public Animator animator;

    private HintData _hintData;

    private bool _enabled = false;

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
    
    public void ShowHint()
    {
        hint.SetActive(true);
        animator.SetTrigger("ShowHint");
        animator.ResetTrigger("HideHint");
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
