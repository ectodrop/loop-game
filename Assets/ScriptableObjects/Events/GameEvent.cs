using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent")]
[Serializable]
public class GameEvent : ScriptableObject
{
    // any class that cares about the event being triggered should listen to this
    // should also remove listener OnDisable
    [NonSerialized]
    protected UnityEvent onTrigger;

    public void OnEnable()
    {
        onTrigger = new UnityEvent();
    }

    public void AddListener(UnityAction call)
    {
        onTrigger.AddListener(call);
    }

    public void RemoveListener(UnityAction call)
    {
        onTrigger.RemoveListener(call);
    }

    public void TriggerEvent()
    {
        onTrigger?.Invoke();
    }
}
