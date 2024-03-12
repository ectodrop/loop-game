using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    public GameObject arrow;
    public TextMeshProUGUI label;
    
    public void Select()
    {
        arrow.SetActive(true);
    }

    public void Deselect()
    {
        arrow.SetActive(false);
    }

    public void SetChoice(string choice)
    {
        label.text = choice;
    }

    public string GetChoice()
    {
        return label.text;
    }
}
