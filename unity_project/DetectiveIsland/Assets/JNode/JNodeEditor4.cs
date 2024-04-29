using Aroka.JsonUtils;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using System;

public class JNodeEditor4 : EditorWindow
{
    private static JNodeInstance jNodeInstance;
    public static JNode JNode
    {
        get => jNodeInstance.jNode;
        set { if (jNodeInstance != null) jNodeInstance.jNode = value; }
    }

    public static float ZoomScale
    {
        get => (float)(jNodeInstance?.zoomScale);
        set { if (jNodeInstance != null) jNodeInstance.zoomScale = value; }
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

    private static Node ConnectStartNode
    {
        get => jNodeInstance?.connectStartNode;
        set { if (jNodeInstance != null) jNodeInstance.connectStartNode = value; }
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
    
    [DidReloadScripts]
    public static void ReloadEditor()
    {
        Debug.Log("Reloaded");
        LoadJNodeInstance();
    }

    private void OnGUI()
    {  
        AutoSaveJNodeInstance();
        DrawGrid();
        DrawJNodeMenuBar();
        EditorControl(Event.current);

        if (JNode != null)
        {
            DrawNodes();
            ProcessEvents(Event.current); 
        }

        DrawJNodeMenuBar();

        if (GUI.changed) Repaint();
    }
    public void AutoSaveJNodeInstance()
    {
        if (jNodeInstance != null && EditorUtility.IsDirty(jNodeInstance))
        {
            Debug.Log("AutoSave");
            jNodeInstance.SaveChanges();
        }
    }

    private void CenterCanvasOnNodes()
    {
        if (JNode != null && JNode.Nodes.Count > 0)
        {
            Vector2 sumPositions = Vector2.zero;
            foreach (Node node in JNode.Nodes)
            {
                sumPositions += node.Rect.center;
            }
            Vector2 averageCenter = sumPositions / JNode.Nodes.Count;
            CanvasOffset = -averageCenter + new Vector2(position.width / 2, position.height / 2);
            Debug.Log("Canvas centered to nodes");
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
            Node node = JNode.Nodes[i];
            node.DrawNode(CanvasOffset);  // 각 노드를 그림

          
        }

        for (int i = 0; i < JNode.Nodes.Count; i++)
        {
            Node node = JNode.Nodes[i];

            if (node.ChildConnectingPoint.isConnected )
            {
                float lineThickness = 5.0f;
                /*
                Debug.Log(node);
                Debug.Log(node.ChildConnectingPoint);
                Debug.Log(node.ChildConnectingPoint.rect);

                Debug.Log(node.ChildConnectingPoint);
                Debug.Log(node.ChildConnectingPoint.ConnectedNodeId);
                Debug.Log(GetNode(node.ChildConnectingPoint.ConnectedNodeId));
                Debug.Log(GetNode(node.ChildConnectingPoint.ConnectedNodeId).ParentConnectingPoint);*/


                // Drawing a line
                Handles.DrawAAPolyLine(lineThickness,
                    new Vector3[] 
                        {
                            node.ChildConnectingPoint.rect.center,
                            GetNode(node.ChildConnectingPoint.ConnectedNodeId).ParentConnectingPoint.rect.center
                        }
                            );
            }
        }


    }


    public static void OpenJNodeEditorWindow()
    {
        JNodeEditor4 window = GetWindow<JNodeEditor4>("J Node Editor 4");
        window.Show();
        LoadJNodeInstance();
        Debug.Log("Open JNode Editor" + window);
    }

    private static void LoadJNodeInstance()
    {
        jNodeInstance = AssetDatabase.LoadAssetAtPath<JNodeInstance>("Assets/JNode/JNodeInstance.asset");
        /*
        if (jNodeInstance == null)
        {
            jNodeInstance = ScriptableObject.CreateInstance<JNodeInstance>();
            AssetDatabase.CreateAsset(jNodeInstance, "Assets/JNode/JNodeInstance.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }*/
    }

    public static void LoadJNodeEditorWindow(string filePath, string _recentOpenFileName)
    {
        Debug.Log("Load JNode   |   " + _recentOpenFileName + "    |    " + filePath);
        JNode jNode = ArokaJsonUtils.LoadJNode(filePath);
        Debug.Log(jNode.Nodes.Count);
        jNodeInstance.Initialize(filePath, _recentOpenFileName, jNode);
        UpdateLastSavedSnapshot();
    }

    public string GetCurrentSnapShot()
    {
        string currentSnapshot = JsonConvert.SerializeObject(JNode, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new Vector2Converter() },
            StringEscapeHandling = StringEscapeHandling.Default
        });
        return currentSnapshot;
    }

    public void DrawJNodeMenuBar()
    {
        Rect buttonArea = new Rect(10, 10, 1000, 30);  // Increased width

        GUILayout.BeginArea(buttonArea);
        GUILayout.BeginHorizontal();

        if (GetCurrentSnapShot() != lastSavedSnapshot)
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
                Scenario scenario = new Scenario( elements);
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
        UpdateLastSavedSnapshot();
        Debug.Log($"<color=green>Save Complete</color> " + RecentOpenFileName);

    }


    public void EditorControl(Event e)
    {
        MousePosition = (e.mousePosition - CanvasOffset);

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            CenterCanvasOnNodes();
            e.Use();
        }

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

    private void DrawConnection(Node parent, Node child)
    {
        Vector2 start = new Vector2(parent.Rect.x + parent.Rect.width / 2, parent.Rect.y + parent.Rect.height);
        Vector2 end = new Vector2(child.Rect.x + child.Rect.width / 2, child.Rect.y);
        Handles.DrawLine(start, end);
    }


    private void ProcessEvents(Event e)
    {
        if (EditorUtility.IsDirty(jNodeInstance))
        {
            return;
        }
        
        MousePosition = (e.mousePosition - CanvasOffset);

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
                    if (SelectedNode != null) SelectedNode.Deselect();
                    SelectedNode = null;

                    foreach (var node in JNode.Nodes)
                    {
                        if (node.ChildConnectingPoint.rect.Contains(MousePosition + CanvasOffset))
                        {
                            Debug.Log("node Cp Click");
                            ConnectStartNode = node;
                            LastMouseDragPosition = e.mousePosition;
                            break;
                        }

                        if (node.Rect.Contains(MousePosition + CanvasOffset))
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
                    SelectedNode.position += e.mousePosition - LastMouseDragPosition;
                    LastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                IsDraggingNode = false;
                IsPanningCanvas = false;

                if (ConnectStartNode != null)
                {
                    bool connnected = false;
                    foreach (var node in JNode.Nodes)
                    {
                        if (node == ConnectStartNode) continue;
                        if (node.ParentConnectingPoint.rect.Contains(MousePosition + CanvasOffset))
                        {
                            Debug.Log("Connect");
                            ConnectStartNode.ConnectNodeToChild(node);
                            node.ConnectNodeToParent(ConnectStartNode);
                         
                            ConnectStartNode = null;
                            LastMouseDragPosition = e.mousePosition;
                            connnected = true;
                            break;
                        }
                    }

                    if (!connnected)
                    {
                        ConnectStartNode.DeConnectNodeChild();
                    }
                }
                ConnectStartNode = null;
                e.Use();
                break;

            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && SelectedNode != null)
                {
                    Debug.Log("Destory Node " + SelectedNode + " | " + JNode.Nodes.Count );
                    if (GetNodeByChild(SelectedNode.ChildConnectingPoint.connectedNodeId) != null)
                        GetNodeByChild(SelectedNode.ChildConnectingPoint.connectedNodeId).DeConnectNodeChild();
                    if (GetNodeByParent(SelectedNode.ParentConnectingPoint.connectedNodeId) != null)
                        GetNodeByParent(SelectedNode.ParentConnectingPoint.connectedNodeId).DeConnectNodeParent();
                    SelectedNode.DeConnectNodeChild();
                    SelectedNode.DeConnectNodeParent();
                    JNode.Nodes.Remove(SelectedNode);
                    Debug.Log("Destory Node " + SelectedNode + " | " + JNode.Nodes.Count);

                    SelectedNode = null;
                    e.Use();
                }
                if (e.keyCode == KeyCode.W && (Event.current.command || Event.current.control))
                {
                    OnDestroy();
                }
                break;

        }
    }

    private void ProcessContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, () => AddDialogueNode(mousePos));
        menu.AddItem(new GUIContent("Add ChoiceSet Node"), false, () => AddChoiceSetNode(mousePos));
        menu.ShowAsContext();
    }

    private void AddDialogueNode(Vector2 position)
    {
        DialogueNode dialogueNode = new DialogueNode(position, "Dialogue");
        dialogueNode.SetGuid();
        dialogueNode.dialogue = new Dialogue("Mono", new List<Line>() { });
        JNode.Nodes.Add(dialogueNode);
    }


    private void AddChoiceSetNode(Vector2 position)
    {
        ChoiceSetNode choiceSetNode = new ChoiceSetNode(position, "ChoiceSet");
        choiceSetNode.SetGuid();
        choiceSetNode.choiceSet = new ChoiceSet(new(),new());
        JNode.Nodes.Add(choiceSetNode);
    }

    public Node GetNode(string id)
    {
        for (int i = 0; i < JNode.Nodes.Count; i++)
        {
            Node node = JNode.Nodes[i];

            if (node.ID == id)
            {
                return node; // Node found
            }
        }
        return null; // No node found with the given ID
    }

    public Node GetNodeByChild(string id)
    {
        for (int i = 0; i < JNode.Nodes.Count; i++)
        {
            Node node = JNode.Nodes[i];

            if (node.ChildConnectingPoint.connectedNodeId == id)
            {
                return node; // Node found
            }
        }
        return null; // No node found with the given ID
    }
    public Node GetNodeByParent(string id)
    {
        for (int i = 0; i < JNode.Nodes.Count; i++)
        {
            Node node = JNode.Nodes[i];

            if (node.ParentConnectingPoint.connectedNodeId == id)
            {
                return node; // Node found
            }
        }
        return null; // No node found with the given ID
    }

    protected virtual void OnClosing()
    {
        Debug.Log("Try Exit");
        if (GetCurrentSnapShot() != lastSavedSnapshot)
        {
            if (EditorUtility.DisplayDialog("Save Changes?",
             "Do you want to save changes to the nodes before closing?",
             "Save", "Don't Save"))
            {
                SaveJNode();  // 사용자가 "Save"를 선택했을 때 저장 함수 호출
            }
            else
            {

            }
        }
    }

    public void OnDestroy()
    {
        OnClosing();
    }

    private static readonly string IconPath = "Assets/Editor/Icons/JNodeIcon.png";  // 아이콘 파일 위치

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

        StartNode startNode = new StartNode(new Vector2(400, 100), "StartNode");
        startNode.SetGuid();
        jNode.Nodes.Add(startNode);

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            StringEscapeHandling = StringEscapeHandling.Default, // Ensuring Hangul is not escaped
            Converters = new List<JsonConverter> {  new Vector2Converter() }
        };

        string json = JsonConvert.SerializeObject(jNode, settings);

        File.WriteAllText(path, json); // Creates an empty JSON object in the file.
        AssetDatabase.Refresh();

        // Load the newly created .jnode file as an asset
        TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

        
        var iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
        if (iconTexture != null && asset != null)
        {
            EditorGUIUtility.SetIconForObject(asset, iconTexture);
        }

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset; // Select the new asset in the project window
    }




}
