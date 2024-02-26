using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialGuide : MonoBehaviour
{
    public TextMeshProUGUI playerThoughts;
    public GameEvent doorFirstClick;
    public ScheduleEvent powerOutageEvent;

    private void OnEnable()
    {
        doorFirstClick.AddListener(HandleDoorFirstClick);
    }

    private void OnDisable()
    {
        doorFirstClick.RemoveListener(HandleDoorFirstClick);
    }

    private void HandleDoorFirstClick()
    {
        StartCoroutine(TriggerPowerOutage());
    }

    IEnumerator TriggerPowerOutage()
    {
        yield return new WaitForSeconds(1);
        powerOutageEvent.TriggerEvent();
        playerThoughts.text = "Oh no the power seems to have gone out. Maybe I need to change the power source.";
    }
}