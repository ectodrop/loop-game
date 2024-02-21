using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedValue<T> : ScriptableObject
{
    public T DefaultValue;

    [NonSerialized]
    private T _currentValue;
    
    private void OnEnable()
    {
        ResetValue();
    }

    private void OnDisable()
    {
        ResetValue();
    }

    public T GetValue()
    {
        return _currentValue;
    }

    public void ResetValue()
    {
        _currentValue = DefaultValue;
    }

    public void SetValue(T val)
    {
        _currentValue = val;
    }
    
    public override string ToString()
    {
        return _currentValue.ToString();
    }
}
