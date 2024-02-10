using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
public class GameEventInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEvent myGameEvent = (GameEvent)target;

        if (GUILayout.Button("TriggerEvent()"))
        {
            myGameEvent.TriggerEvent();
        }
    }
}

