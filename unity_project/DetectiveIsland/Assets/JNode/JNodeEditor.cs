using Aroka.JsonUtils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;

public class JNodeEditor : EditorWindow
{ 
    /*
    private void OnDestroy()
    {
        jNodeInstance.isOpened = false;
    }
   
    private void OnEnable()
    {
        CreateJNodeInstance();
        if (!jNodeInstance.isOpened)
        {
            return;
        }
        jNodeInstance = AssetDatabase.LoadAssetAtPath<JNodeInstance>("Assets/JNode/JNodeInstance.asset");
        LoadJNodeEditorWindow(jNodeInstance.recentPath, jNodeInstance.recentPath);
    }*/

    private static JNodeInstance jNodeInstance;

    public static JNode JNode
    {
        get => jNodeInstance.jNode;
        set { if (jNodeInstance != null) jNodeInstance.jNode = value;}
    }
    public static string RecentOpenFileName
    {
        get => jNodeInstance?.recentOpenFileName;
        set { if (jNodeInstance != null) jNodeInstance.recentOpenFileName = value; }
    }
    private static Node SelectedNode
    {
        get => jNodeInstance?.selectedNode;
        set { if (jNodeInstance != null) jNodeInstance.selectedNode = value; }
    }
    private static Vector2 MousePosition
    {
        get => jNodeInstance?.mousePosition ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.mousePosition = value; }
    }
    private static Vector2 LastMouseDragPosition
    {
        get => jNodeInstance?.lastMouseDragPosition ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.lastMouseDragPosition = value; }
    }
    private static Vector2 CanvasOffset
    {
        get => jNodeInstance?.canvasOffset ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.canvasOffset = value; }
    }
    private static bool IsDraggingNode
    {
        get => jNodeInstance != null && jNodeInstance.isDraggingNode;
        set { if (jNodeInstance != null) jNodeInstance.isDraggingNode = value; }
    }
    private static bool IsPanningCanvas
    {
        get => jNodeInstance != null && jNodeInstance.isPanningCanvas;
        set { if (jNodeInstance != null) jNodeInstance.isPanningCanvas = value; }
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawJNodeMenuBar();
        EditorControl(Event.current);

        if (JNode != null)
        {
            DrawNodes();
            ProcessEvents(Event.current);
        }

        if (GUI.changed) Repaint();
    }


    private static void CreateJNodeInstance()
    { 
        if (jNodeInstance == null)
        {
            jNodeInstance = ScriptableObject.CreateInstance<JNodeInstance>();
            AssetDatabase.CreateAsset(jNodeInstance, "Assets/JNode/JNodeInstance.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        jNodeInstance = AssetDatabase.LoadAssetAtPath<JNodeInstance>("Assets/JNode/JNodeInstance.asset");
    }

    public void EditorControl(Event e)
    {
       
        MousePosition = (e.mousePosition - CanvasOffset);

        switch (e.type)
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.W && (Event.current.command || Event.current.control))
                {
                    Event.current.Use();
                    Close();
                }
                break;

        }
    }


    public static void OpenJNodeEditorWindow()
    {
        Debug.Log("Open JNode Editor");
        JNodeEditor window = GetWindow<JNodeEditor>("JNode Editor");
        CreateJNodeInstance();
        window.Show();
    }

    public static void LoadJNodeEditorWindow(string filePath, string _recentOpenFileName)
    {
        Debug.Log("Load JNode   |   " + _recentOpenFileName + "    |    " + filePath);
        JNode jNode = ArokaJsonUtils.LoadJNode(filePath);
        jNodeInstance.Initialize(filePath, _recentOpenFileName, jNode);
        UpdateLastSavedSnapshot();
    }


    public void DrawJNodeMenuBar()
    {

        Rect buttonArea = new Rect(10, 10, 1000, 30);  // Increased width

        GUILayout.BeginArea(buttonArea);
        GUILayout.BeginHorizontal();

        string currentSnapshot = JsonConvert.SerializeObject(JNode, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new Vector2Converter() },
            StringEscapeHandling = StringEscapeHandling.Default
        });

        if (currentSnapshot != lastSavedSnapshot)
        {
            GUI.color = Color.red;  // Green color for the Save button
        }
        else
        {
            GUI.color = Color.white;  // Green color for the Save button
        }
        if (GUILayout.Button("Save", GUILayout.Width(90), GUILayout.Height(20)))  // Ensure the button has enough space
        {
            SaveJNode();
        }

        GUI.color = Color.white;  // Green color for the Save button
        if (GUILayout.Button("Save As", GUILayout.Width(90), GUILayout.Height(20)))  // Ensure the button has enough space
        {
            SaveAsJNode();
        }

        GUI.color = Color.white;
        if (GUILayout.Button("Export", GUILayout.Width(90), GUILayout.Height(20)))  // Ensure the button has enough space
        {
            ExportJson();
        }

        // Set the color for the label displaying the number of nodes
        GUI.color = Color.white; // Reset color to default

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private static string lastSavedSnapshot;
    public static void UpdateLastSavedSnapshot()
    {
        lastSavedSnapshot = JsonConvert.SerializeObject(JNode, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new Vector2Converter() },
            StringEscapeHandling = StringEscapeHandling.Default
        });
    }



    private void CloseJNode()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.W && (Event.current.command || Event.current.control))
            {
                Event.current.Use();

                Close();
            }
        }
    }


    [MenuItem("Assets/Create/JNode/New JNode", false, 80)]
    public static void CreateNewJNode()
    {
        string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(folderPath))
        {
            folderPath = Path.GetDirectoryName(folderPath);
        }

        string path = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewJNode.jnode");

        JNode jNode = new JNode(new List<Node>());
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            StringEscapeHandling = StringEscapeHandling.Default // Ensuring Hangul is not escaped
        };

        string json = JsonConvert.SerializeObject(jNode, settings);

        File.WriteAllText(path, json); // Creates an empty JSON object in the file.
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
    }

    public static void SaveJNode()
    {
        string currentSnapshot = JsonConvert.SerializeObject(JNode, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new Vector2Converter() },
            StringEscapeHandling = StringEscapeHandling.Default
        });

        if (currentSnapshot != lastSavedSnapshot)
        {
            string path2 = Path.Combine(StoragePath.JNodePath, RecentOpenFileName);
            if (!string.IsNullOrEmpty(path2))
            {
                File.WriteAllText(path2, currentSnapshot);
                Save(path2);
                lastSavedSnapshot = currentSnapshot;  // Update the snapshot after saving
            }
        }
        else
        {
            Debug.Log("No changes to save.");
        }
    }

    public static void SaveAsJNode()
    {
        string resourcesPath = StoragePath.JNodePath;

        string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

        // Open the save file dialog with the determined initial directory
        string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", StoragePath.JNodePath, RecentOpenFileName, "jnode");
        Debug.Log(path2);
        //C:/Users/acy04/Desktop/RecentProject/DetectiveIsland/unity_project/DetectiveIsland/Assets/Resources/ScenarioNodes/NewScenarioNode2 1.jnode
        //"C:\Users\acy04\Desktop\RecentProject\DetectiveIsland\unity_project\DetectiveIsland\Assets\Resources\ScenarioNodes\NewScenarioNode2 1.jnode\.jnode".
        // Check if the user has not cancelled the operation
        if (!string.IsNullOrEmpty(path2))
        {
            Debug.Log("Nodes saved to JSON: " + path2);
            Save(path2);
        }
    }

    public static void Save(string path)
    {
        ArokaJsonUtils.SaveJNode(JNode, path);
        Debug.Log($"<color=green>Save Complete</color> " + RecentOpenFileName);
    }

    private void ExportJson()
    {
        if (JNode.Nodes.Count > 0)
        {

            string resourcesPath = StoragePath.ScenarioPath;

            string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

            // Open the save file dialog with the determined initial directory
            string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", initialDirectory, "TestJson", "json");

            // Check if the user has not cancelled the operation
            if (!string.IsNullOrEmpty(path2))
            {
                List<Element> elements = JNode.Nodes.ToElements();
                Debug.Log(elements.Count);
                Scenario scenario = new Scenario(null,elements);
                Debug.Log(scenario.Elements.Count);

                // Save the scenario object as a JSON file at the specified path
                ArokaJsonUtils.SaveScenario(scenario, path2);
                Debug.Log("Nodes saved to JSON: " + path2);
            }
        }
        else
        {
            Debug.Log("No nodes to save.");
        }
    }








    private void DrawGrid()
    {
        float gridSize = 20f;
        float gridOpacity = 0.2f;
        int widthDivs = Mathf.CeilToInt(position.width / gridSize);
        int heightDivs = Mathf.CeilToInt(position.height / gridSize);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, gridOpacity);

        // Calculate the offset to start drawing the grid lines based on the canvasOffset
        Vector2 offset = new Vector2(CanvasOffset.x % gridSize, CanvasOffset.y % gridSize);

        for (int i = 0; i <= widthDivs; i++)
        {
            // Calculate the start and end points for vertical grid lines
            float x = gridSize * i + offset.x;
            Handles.DrawLine(new Vector2(x, 0), new Vector2(x, position.height));
        }

        for (int j = 0; j <= heightDivs; j++)
        {
            // Calculate the start and end points for horizontal grid lines
            float y = gridSize * j + offset.y;
            Handles.DrawLine(new Vector2(0, y), new Vector2(position.width, y));
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        for (int i = 0; i < JNode.Nodes.Count; i++)
        {
            JNode.Nodes[i].DrawNode(CanvasOffset);  // 각 노드를 그림

        }
        
    }




    private void ProcessEvents(Event e)
    {
        MousePosition = (e.mousePosition - CanvasOffset) ;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 2) // Middle mouse button for panning
                {
                    IsPanningCanvas = true;
                    LastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (e.button == 1) // Right click for context menu
                {
                    ProcessContextMenu(e.mousePosition);
                    e.Use();
                }
                else if (e.button == 0) // Left mouse button for selecting and dragging nodes
                {
                   if(SelectedNode != null) SelectedNode.Deselect();
                    SelectedNode = null;

                    foreach (var node in JNode.Nodes)
                    {
                        if (node.rect.Contains(MousePosition))
                        {
                            SelectedNode = node;
                            SelectedNode.Select();
                            IsDraggingNode = true;
                            LastMouseDragPosition = e.mousePosition;
                            break;
                        }
                    }
                    IsPanningCanvas = false;

                    if (SelectedNode == null)
                    {
                        LastMouseDragPosition = e.mousePosition;
                    }
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (IsPanningCanvas)
                {
                    CanvasOffset += e.mousePosition - LastMouseDragPosition;
                    LastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (IsDraggingNode && SelectedNode != null)
                {
                    SelectedNode.rect.position += e.mousePosition - LastMouseDragPosition;
                    LastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                IsDraggingNode = false;
                IsPanningCanvas = false;
                e.Use();
                break;

            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && SelectedNode != null)
                {
                    JNode.Nodes.Remove(SelectedNode);
                    SelectedNode = null;
                    e.Use();
                }
                if (e.keyCode == KeyCode.W && (Event.current.command || Event.current.control))
                {
                    Event.current.Use();
                    Close();
                }
                break;
              
        }
    }




    private void ProcessContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, () => AddDialogueNode(mousePos));
        menu.ShowAsContext();


    }

    private void AddDialogueNode(Vector2 position)
    {
        DialogueNode dialogueNode = new DialogueNode(new Rect(position.x, position.y, 200, 100), "Dialogue");
        dialogueNode.dialogue = new Dialogue("Kate", new List<Line>() { new Line("smile", "Hello") });
        JNode.Nodes.Add(dialogueNode);
    }
}