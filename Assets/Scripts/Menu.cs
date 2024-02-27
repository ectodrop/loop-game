using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnTutorialButton()
    {
        SceneManager.LoadScene("Tutorial-Power");
    }
    public void OnTutorialButton1()
    {
        SceneManager.LoadScene("Tutorial-emily");
    }
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Level One");
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
