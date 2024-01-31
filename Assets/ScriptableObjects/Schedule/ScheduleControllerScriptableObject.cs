using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleControllerSO", menuName = "ScriptableObjects/ScheduleControllerScriptableObject")]
public class ScheduleControllerScriptableObject : ScriptableObject
{
    public ScheduleEventScriptableObject[] scheduledEvents;

    public void OnEnable()
    {
        Array.Sort(scheduledEvents, delegate (ScheduleEventScriptableObject x, ScheduleEventScriptableObject y) {
            return x.time.CompareTo(y.time);
        });
    }
}