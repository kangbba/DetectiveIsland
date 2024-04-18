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
        // Assuming NodeEditorWindow is the window you want to open
        var window = EditorWindow.GetWindow<JNodeEditor>("JNode Editor");
        string fileContents = System.IO.File.ReadAllText(path);
        // Assuming LoadNodesFromJson is a method designed to handle the JSON data
        // This line will need adjustment if LoadNodesFromJson doesn't exist or needs different parameters
        string filename = System.IO.Path.GetFileName(path);

        JNodeEditor.OpenJNodeEditorWindow();
        window.Show();
        JNodeEditor.LoadJNodeEditor(path, filename);
    }
}
