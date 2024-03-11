using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject controlPanelUI;
    public GameObject loginPanelUI;
    [Header("Triggers")]
    public GameEvent HUDEnableEvent;
    public GameEvent HUDDisableEvent;
    public GameEvent resetLoopEvent;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        // If both UI panels are closed, continue timer and hide cursor
        if (!loginPanelUI.activeInHierarchy && !controlPanelUI.activeInHierarchy)
        {
            HUDEnableEvent.TriggerEvent();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void PauseGame()
    {
        HUDDisableEvent.TriggerEvent();
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // start loop
    public void RestartLoop()
    {
        resetLoopEvent.TriggerEvent();
        ResumeGame();
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
