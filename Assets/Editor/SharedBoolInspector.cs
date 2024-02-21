using UnityEditor;

[CustomEditor(typeof(SharedBool))]
public class SharedBoolInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SharedBool sharedBool = (SharedBool)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Value");
        EditorGUILayout.LabelField(sharedBool.ToString());
        EditorGUILayout.EndHorizontal();
    }
}
