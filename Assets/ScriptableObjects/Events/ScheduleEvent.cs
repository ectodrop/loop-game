using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleEvent", menuName = "ScriptableObjects/ScheduleEvent")]
[Serializable]
public class ScheduleEvent : GameEvent
{
    [Tooltip("Number of seconds from the start when the event should be triggered")]
    public float time;
}