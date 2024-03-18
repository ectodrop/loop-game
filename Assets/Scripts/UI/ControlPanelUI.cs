using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    public GameObject controlPanelUI;
    public TimeLoopController _timeLoopController;

    [Header("Triggers")]
    public GameEvent HUDEnableEvent;
    public GameEvent HUDDisableEvent;
    [Header("Listening To")]
    public GameEvent controlPanelInteracted;

    public void OnEnable()
    {
        controlPanelInteracted.AddListener(OnControlPanelInteracted);
    }

    public void OnDisable()
    {
        controlPanelInteracted.RemoveListener(OnControlPanelInteracted);
    }

    public void OnControlPanelInteracted()
    {
        HUDDisableEvent.TriggerEvent();
        controlPanelUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _timeLoopController.StopTime();
    }

    public void CloseControlPanel()
    {
        HUDEnableEvent.TriggerEvent();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlPanelUI.SetActive(false);
        _timeLoopController.ResumeTime();
    }
}
