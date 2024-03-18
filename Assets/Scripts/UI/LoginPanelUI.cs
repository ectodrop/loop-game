using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;

public class LoginPanelUI : MonoBehaviour
{
    public GameObject loginPanelUI;
    public TimeLoopController _timeLoopController;
    
    [Header("Listening To")]
    public GameEvent LoginUIOn;

    [Header("Triggers")]
    public GameEvent HUDEnableEvent;
    public GameEvent HUDDisableEvent;

    public void OnEnable()
    {
        LoginUIOn.AddListener(OnLoginPanel);
    }

    public void OnDisable()
    {
        LoginUIOn.RemoveListener(CloseLoginPanel);
    }

    public void OnLoginPanel()
    {
        HUDDisableEvent.TriggerEvent();
        loginPanelUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _timeLoopController.StopTime();
    }

    public void CloseLoginPanel()
    {
        HUDEnableEvent.TriggerEvent();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loginPanelUI.SetActive(false);
        _timeLoopController.ResumeTime();
    }
}
