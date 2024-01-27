using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractGUIController : MonoBehaviour
{
    public TextMeshProUGUI interactableLabel;
    public void Show(string text)
    {
        interactableLabel.text = text;
    }

    public void Hide()
    {
        interactableLabel.text = "";
    }
}
