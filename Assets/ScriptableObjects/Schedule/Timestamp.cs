using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Timestamp
{
    [SerializeField] private int hour;
    [SerializeField] private int minute;
    private float second;

    public Timestamp(int hour, int minute)
    {
        this.hour = hour;
        this.minute = minute;
    }

    // need a custom mod here because modulus operator in c# is not a real modulus operator
    // this mod works with negative numbers
    private int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r+m : r;
    }
    
    // works with negative amount
    public void AddHours(int amount)
    {
        hour = mod(hour + amount, 24);
    }

    // works with negative amount
    public void AddMinutes(int amount)
    {
        AddHours(Mathf.FloorToInt((minute + amount)/60f));
        minute = mod(minute + amount, 60);
    }

    // works with negative amount
    public void AddSeconds(float amount)
    {
        AddMinutes(Mathf.FloorToInt((second + amount)/60f));
        second += amount;
        if (second > 60f || second < 0f)
            second = mod((int)second, 60);
    }

    public int MinutesUntil(Timestamp other)
    {
        // this timestamp is after the other timestamp
        if (this.CompareTo(other) >= 0) return 0;

        if (hour == other.hour) return other.minute - minute;

        return (other.hour - hour) * 60 + (60 - minute) + (other.minute);
    }

    public void SetHour(int hour)
    {
        this.hour = hour;  
    }

    public void SetMinute(int minute)
    {
        this.minute = minute;
    }

    public int CompareTo(Timestamp other)
    {
        if (this.hour == other.hour) return this.minute.CompareTo(other.minute);
        
        return this.hour.CompareTo(other.hour);
    }
    
    public override string ToString()
    {
        return $"{hour:00}:{minute:00}";
    }

    public Timestamp Clone()
    {
        return new Timestamp(hour, minute);
    }

    public string ToStringAMPM()
    {
        if (hour == 0) return $"12:{minute:00} AM";
        if (hour == 12) return $"12:{minute:00} PM";
        if (hour > 12) return $"{hour/12:00}:{minute:00} PM";
        return $"{hour:00}:{minute:00} AM";
    }
}
