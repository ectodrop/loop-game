using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeLimitControllerSO", menuName = "ScriptableObjects/TimeLimitControllerScriptableObject")]
public class TimeLimitControllerScriptableObject : ScriptableObject
{
    public float defaultMaxTime = 10.0f;
    [NonSerialized]
    public float currentMaxTime;

    private void OnEnable()
    {
        currentMaxTime = defaultMaxTime;
    }

    public void ExtendTimeLimit(float extraTime)
    {
        currentMaxTime += extraTime;
    }
}