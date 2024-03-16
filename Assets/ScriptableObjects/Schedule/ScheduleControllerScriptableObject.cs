using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleControllerSO", menuName = "ScriptableObjects/ScheduleControllerScriptableObject")]
public class ScheduleControllerScriptableObject : ScriptableObject
{
    public ScheduleEvent[] scheduledEvents;

    public void OnEnable()
    {
        if (scheduledEvents != null)
            Array.Sort(scheduledEvents, (x, y) => x.time.CompareTo(y.time) );
    }
}