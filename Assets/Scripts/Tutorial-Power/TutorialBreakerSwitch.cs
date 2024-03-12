using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBreakerSwitch : MonoBehaviour, IInteractable, ILabel
{
    public string GetLabel()
    {
        return "Breaker Switch [E]";
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        SceneManager.LoadScene("Level One");
    }
}
