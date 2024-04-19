using UnityEngine;
using TMPro; // Namespace for TextMeshPro
using UnityEngine.UI;

public class CoolantSystem : MonoBehaviour
{
    public GameObject controlPanelUI;

    [Header("Triggers")]
    public GameEvent coolantDrainedEvent;
    public GameEvent HUDEnableEvent;

    public void DrainCoolant()
    {
        // Perform the coolant drain
        coolantDrainedEvent.TriggerEvent();
        Debug.Log("Coolant is being drained.");
        HUDEnableEvent.TriggerEvent();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlPanelUI.SetActive(false);
    }
}

