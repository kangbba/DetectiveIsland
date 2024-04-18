using UnityEngine;
using UnityEditor;

public class JNodeIconSetter : AssetPostprocessor
{
    // Path to the icon, make sure to adjust this to where you've saved your custom icon
    private static readonly string iconPath =  "Assets/JNode/Icons/jnode_icon.png";

    // This method gets called whenever an asset is imported, deleted, or moved within the editor
    private static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string path in importedAssets)
        {
            AssignIcon(path);
        }

        foreach (string path in movedAssets)
        {
            AssignIcon(path);
        }
    }

    private static void AssignIcon(string assetPath)
    {
        Debug.Log("Checking asset: " + assetPath);
        if (assetPath.EndsWith(".jnode"))
        {
            Debug.Log("Found .jnode file: " + assetPath);
            var iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            if (iconTexture == null)
            {
                Debug.LogError("Icon texture not loaded from: " + iconPath);
                return;
            }

            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset == null)
            {
                Debug.LogError("No main asset found at path: " + assetPath);
                return;
            }

            EditorGUIUtility.SetIconForObject(asset, iconTexture);
            Debug.Log("Icon set for asset: " + assetPath);
        }
    }

}
