using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ScheduleEventSO", menuName = "ScriptableObjects/ScheduleEventScriptableObject")]
public class ScheduleEventScriptableObject : ScriptableObject
{
    [Tooltip("Number of seconds from the start when the event should be triggered")]
    public float time;

    // any class that cares about the scheduled event being triggered should listen to this
    // should also remove listener OnDisable
    public UnityEvent onScheduleTrigger { get; private set; }

    public void OnEnable()
    {
        onScheduleTrigger = new UnityEvent();
    }

    public void TriggerScheduled()
    {
        onScheduleTrigger?.Invoke();
    }
}