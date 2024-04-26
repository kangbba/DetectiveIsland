using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class JNodeIconAssigner : AssetPostprocessor
{
    private static readonly string IconPath = "Assets/Editor/Icons/JNodeIcon.png";  // Update this path to where your icon is located.

    // This method is called whenever an asset is imported, deleted, or moved.
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string path in importedAssets)
        {
            AssignIcon(path);
        }
    }

    private static void AssignIcon(string assetPath)
    {
        bool endsWithJNode = assetPath.EndsWith(".jnode", StringComparison.OrdinalIgnoreCase);

        if (endsWithJNode)
        {
            var iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (iconTexture == null)
            {
                return;
            }

            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset != null)
            {
                EditorGUIUtility.SetIconForObject(asset, iconTexture);
            }
            else
            {
            }
        }


    }
}
