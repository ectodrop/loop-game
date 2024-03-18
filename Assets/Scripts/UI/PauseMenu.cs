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
    public GameObject soundMenuUI;
    public TimeLoopController _timeLoopController;

    public GameObject controlMenuUI;

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
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !pauseMenuUI.activeInHierarchy && !soundMenuUI.activeInHierarchy && !controlMenuUI.activeInHierarchy)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        BacktoGame();
    }

    void PauseGame()
    {
        HUDDisableEvent.TriggerEvent();
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _timeLoopController.StopTime();
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

    public void OpenSoundMenu()
    {
        soundMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void ReturnToPauseMenu()
    {
        if (soundMenuUI.activeInHierarchy)
        {
            soundMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
        else if (controlMenuUI.activeInHierarchy)
        {
            controlMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

    public void ExitfromSoundSetting()
    {
        soundMenuUI.SetActive(false);
        BacktoGame();
    }

    public void OpenControlMenu()
    {
        controlMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void ExitfromControlMenu()
    {
        controlMenuUI.SetActive(false);
        BacktoGame();
    }

    public void BacktoGame()
    {
        // If both UI panels are closed, continue timer and hide cursor
        if (!loginPanelUI.activeInHierarchy && !controlPanelUI.activeInHierarchy)
        {
            HUDEnableEvent.TriggerEvent();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _timeLoopController.ResumeTime();
        }
    }
}