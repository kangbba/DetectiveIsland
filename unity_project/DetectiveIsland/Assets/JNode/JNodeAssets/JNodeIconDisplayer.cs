using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class JNodeIconDisplayer
{
    private static readonly string IconPath = JNodePaths.JNodeIconPath;
    static Texture2D myIcon;
    static bool iconLoadAttempted = false;

    static JNodeIconDisplayer()
    {
        LoadIcon();

        // Hook into the project window drawing event
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    static void LoadIcon()
    {
        if (iconLoadAttempted) return;

        myIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);

        if (myIcon == null)
        {
            Debug.LogWarning($"Failed to load icon at path: {IconPath}. Retrying...");
            EditorApplication.delayCall += LoadIcon;
        }
        else
        {
            Debug.Log($"Icon loaded successfully at path: {IconPath}");
            iconLoadAttempted = true;
        }
    }

    static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        if (myIcon == null && !iconLoadAttempted)
        {
            // Attempt to reload the icon if it is null and not yet attempted
            LoadIcon();

            if (myIcon == null)
            {
                Debug.LogError($"Failed to load icon at path: {IconPath}");
                return;
            }
        }

        // Get the asset path using the GUID
        string path = AssetDatabase.GUIDToAssetPath(guid);

        // Check if the asset path ends with .jnode
        if (path.EndsWith(".jnode"))
        {
            // Draw the custom icon
            GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height), myIcon);
        }
    }
}
