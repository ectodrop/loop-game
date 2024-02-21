using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TimeSettings", menuName = "ScriptableObjects/TimeSettings")]
public class TimeSettings : ScriptableObject
{
    public Timestamp defaultStartTimestamp = new Timestamp(12, 0);
    public Timestamp defaultEndTimestamp = new Timestamp(12, 20);
    
    [Tooltip("How many seconds of realtime is 1 minute ingame?")]
    [Min(0.1f)]
    public float secondsPerMinute = 1.0f;
    
    [Tooltip("How many ingame minutes need to pass for the timer text to be updated?")]
    [Min(1)]
    public int minutesPerIncrement = 5;

    [NonSerialized]
    public float currentTimeSeconds;

    [NonSerialized]
    public Timestamp currentTimestamp;

    [NonSerialized]
    public Timestamp currentStartTimestamp;
    [NonSerialized]
    public Timestamp currentEndTimestamp;

    private void OnEnable()
    {
        currentStartTimestamp = defaultStartTimestamp.Clone();
        currentEndTimestamp = defaultEndTimestamp.Clone();
        currentTimestamp = currentStartTimestamp.Clone();
        ResetTimers();
    }

    public float SecondsPerIncrement()
    {
        return secondsPerMinute * minutesPerIncrement;
    }

    public int NextIncrement()
    {
        return (int)(currentTimeSeconds / SecondsPerIncrement()) + 1;
    }

    public float CurrentMaxTimeSeconds()
    {
        // number of minutes in a loop * seconds in a minute
        return currentStartTimestamp.MinutesUntil(currentEndTimestamp) * secondsPerMinute;
    }
    public float SecondsIntoIncrement()
    {
        int prevIncrement = (int)(currentTimeSeconds / SecondsPerIncrement());
        return currentTimeSeconds - (prevIncrement * SecondsPerIncrement());
    }

    public void ResetTimers()
    {
        // consistency checking
        if (CurrentMaxTimeSeconds() % (int)SecondsPerIncrement() > 0)
        {
            Debug.LogError("Invalid: Pick a different max time or increment");
        }
        currentTimeSeconds = 0;
        currentTimestamp = currentStartTimestamp.Clone();
    }
}