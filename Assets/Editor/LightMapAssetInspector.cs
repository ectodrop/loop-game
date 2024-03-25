using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightMapAsset), editorForChildClasses: true)]
public class LightMapAssetInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LightMapAsset lightMapAsset = (LightMapAsset)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Light Map Data Length");
        EditorGUILayout.LabelField(lightMapAsset.GetLightMapData().Length.ToString());
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Lightmaps"))
        {
            lightMapAsset.Clear();
        }
    }
}

