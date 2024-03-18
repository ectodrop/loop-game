using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopTutorialGuide : MonoBehaviour
{
    public DoorScript timedDoor;
    [Header("Listening To")]
    public ScheduleEvent doorCloseEvent;
    

    private void OnEnable()
    {
        doorCloseEvent.AddListener(CloseDoor);
    }

    private void OnDisable()
    {
        doorCloseEvent.RemoveListener(CloseDoor);
    }

    private void CloseDoor()
    {
        timedDoor.CloseDoor();
    }
}
