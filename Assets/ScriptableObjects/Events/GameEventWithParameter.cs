using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventWithParameter<T> : ScriptableObject
{
    // any class that cares about the event being triggered should listen to this
    // should also remove listener OnDisable
    [NonSerialized]
    protected UnityEvent<T> onTrigger;

    public void OnEnable()
    {
        onTrigger = new UnityEvent<T>();
    }

    public void AddListener(UnityAction<T> call)
    {
        onTrigger.AddListener(call);
    }

    public void RemoveListener(UnityAction<T> call)
    {
        onTrigger.RemoveListener(call);
    }

    public void TriggerEvent(T parameter)
    {
        onTrigger?.Invoke(parameter);
    }
}
