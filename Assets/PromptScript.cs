using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptScript : MonoBehaviour
{
    public GameObject hint;
    public TextMeshProUGUI label;
    public Animator animator;
    


    public void ShowHint(string labelString)
    {
        hint.SetActive(true);
        label.text = labelString;
        animator.SetTrigger("ShowHint");
        animator.ResetTrigger("HideHint");
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }
}
