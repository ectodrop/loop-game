using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTeleporter : MonoBehaviour
{
    public string menuName;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Exiting Power Tutorial.");

        // Return to Main Menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(menuName);

    }
}
