using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeSettings))]
public class TimeSettingsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TimeSettings settings = (TimeSettings)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Seconds Per Increment");
        EditorGUILayout.LabelField(settings.SecondsPerIncrement().ToString());
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max Time In Seconds");
        int totalMinutes = settings.defaultStartTimestamp.MinutesUntil(settings.defaultEndTimestamp); 
        EditorGUILayout.LabelField((totalMinutes * settings.secondsPerMinute).ToString());
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Total number of increments");
        EditorGUILayout.LabelField((totalMinutes / settings.minutesPerIncrement).ToString());
        EditorGUILayout.EndHorizontal();
    }
}
