using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

// This class uses the AssetPostprocessor to hook into Unity's asset import events.
public class AssetImportListener : AssetPostprocessor
{
    private const string targetFolder = "Assets/Scenes/";
    // This method is called whenever assets are imported, deleted, or moved within the Editor.
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // Check if any of the imported assets are within the target folder.
        foreach (string path in importedAssets)
        {
            if (path.StartsWith(targetFolder) && path.Contains("-LightMaps") && (path.Contains(".png") || path.Contains(".exr")))
                CreateOrUpdateScriptableObject(Path.GetDirectoryName(path));
        }
    }

    private static void CreateOrUpdateScriptableObject(string folderPath)
    {
        var path = folderPath.Split(Path.DirectorySeparatorChar).Reverse().ToArray();
        string assetName = path[1].Replace(" ", "-") + "_" + path[0] + ".asset";
        // Find the existing ScriptableObject or create a new one.
        LightMapAsset lightMapAsset = AssetDatabase.LoadAssetAtPath<LightMapAsset>(Path.Join(folderPath, assetName));
        if (lightMapAsset == null)
        {
            lightMapAsset = ScriptableObject.CreateInstance<LightMapAsset>();
            AssetDatabase.CreateAsset(lightMapAsset, Path.Join(folderPath, assetName));
        }

        lightMapAsset.Clear();
        // Find all textures in the folder and add them to the list.
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);

        foreach (FileInfo file in files)
        {
            if (file.Extension == ".png" || file.Extension == ".exr")
            {
                string assetPath = "Assets" + file.FullName.Substring(Application.dataPath.Length);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture != null)
                {
                    if (file.Name.Contains("_dir"))
                        lightMapAsset.lightMapDirs.Add(texture);
                    else if (file.Name.Contains("_light"))
                        lightMapAsset.lightMapColors.Add(texture);
                }
            }
        }

        // Save the changes to the ScriptableObject.
        EditorUtility.SetDirty(lightMapAsset);
        AssetDatabase.SaveAssets();
        Debug.Log("TextureCollection ScriptableObject updated.");
    }
}