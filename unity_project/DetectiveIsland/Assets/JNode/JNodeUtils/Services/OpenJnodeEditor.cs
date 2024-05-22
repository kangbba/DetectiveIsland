using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class OpenJNodeEditor
{
    static OpenJNodeEditor()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
    }

    private static void OnProjectWindowGUI(string guid, Rect selectionRect)
    {
        // Check if the event is a mouse down event and the click count is 2 (double-click)
        if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 2 && selectionRect.Contains(Event.current.mousePosition))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.EndsWith(".jnode"))
            {
                OpenJNodeFile(path);
                Event.current.Use(); // Consume the event to prevent other actions.
            }
        }
    }

    private static void OpenJNodeFile(string path)
    {
        Debug.Log("Try open JNode"); 
        string filename = System.IO.Path.GetFileName(path);
        JNodeEditor4.OpenJNodeEditorWindow(path, filename);
    }
}
