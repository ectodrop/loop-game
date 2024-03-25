using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerScript : MonoBehaviour, IInteractable, ILabel
{
    public string goToScene;
    public void Interact()
    {
        // trigger win animation
        SceneManager.LoadScene(goToScene);
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetLabel()
    {
        return "Collect Flower [E]";
    }
}
