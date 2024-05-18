using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[InitializeOnLoad]

public class JNodeIconDisplayer 
{
    static Texture2D myIcon;

    static JNodeIconDisplayer()
    {
        // Load your custom icon
        myIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/JNode/Textures/JNodeIcon2.png");
        // Hook into the project window drawing event
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        // Get the asset path using the GUID
        string path = AssetDatabase.GUIDToAssetPath(guid);
        // Check if the asset path ends with .txt
        if (path.EndsWith(".jnode"))
        {
            // Draw the custom icon
            GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height), myIcon);
        }
    }



}
