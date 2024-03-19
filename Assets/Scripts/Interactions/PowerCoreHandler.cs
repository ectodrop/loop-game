using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerCoreHandler : MonoBehaviour, IInteractable
{
    [Header("Triggers")]
    public GameEventInt timeExtensionEvent;
    public void Interact()
    {
        // timeExtensionEvent.TriggerEvent(10);
        SceneManager.LoadScene("Level Two");
    }

    public bool CanInteract()
    {
        return true;
    }
}
