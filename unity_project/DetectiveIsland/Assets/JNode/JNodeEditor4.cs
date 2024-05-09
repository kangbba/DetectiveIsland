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
using System.Linq;

public class JNodeEditor4 : EditorWindow
{
    private static JNodeInstance jNodeInstance;

    public static List<Node> Nodes
    {
        get => (jNodeInstance?.Nodes);
        set { if (jNodeInstance != null) jNodeInstance.Nodes = value; }
    }
    public static string RecentOpenFileName
    {
        get => jNodeInstance?.recentOpenFileName;
        set { if (jNodeInstance != null) jNodeInstance.recentOpenFileName = value; }
    }
    private static Node SelectedNode
    {
        get => jNodeInstance? .editorUIState.selectedNode;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.selectedNode = value; }
    }

    private static Node ConnectStartNode
    {
        get => jNodeInstance?.editorUIState.connectStartNode;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.connectStartNode = value; }
    }
    private static Vector2 MousePosition
    {
        get => jNodeInstance?.editorUIState.mousePosition ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.mousePosition = value; }
    }
    private static Vector2 LastMouseDragPosition
    {
        get => jNodeInstance?.editorUIState.lastMouseDragPosition ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.lastMouseDragPosition = value; }
    }
    private static Vector2 CanvasOffset
    {
        get => jNodeInstance?.editorUIState.canvasOffset ?? Vector2.zero;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.canvasOffset = value; }
    }
    private static bool IsDraggingNode
    {
        get => jNodeInstance != null && jNodeInstance.editorUIState.isDraggingNode;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.isDraggingNode = value; }
    }
    private static bool IsPanningCanvas
    {
        get => jNodeInstance != null && jNodeInstance.editorUIState.isPanningCanvas;
        set { if (jNodeInstance != null) jNodeInstance.editorUIState.isPanningCanvas = value; }
    }
    
    [DidReloadScripts]
    public static void ReloadEditor()
    {
        Debug.Log("Reloaded");
        LoadJNodeInstance(); 
           
        LoadJNodeEditorWindow(jNodeInstance.recentPath ,RecentOpenFileName);
        UpdateLastSavedSnapshot(); 
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawJNodeMenuBar();
        EditorControl(Event.current);
        if (jNodeInstance != null)
        {
            DrawNodes();
            ProcessEvents(Event.current); 
        }
        DrawJNodeMenuBar();
        if (GUI.changed) Repaint();
        AutoSaveJNodeInstance();

    }
    public void AutoSaveJNodeInstance()
    {
          
        if (jNodeInstance != null && EditorUtility.IsDirty(jNodeInstance))
        {
            jNodeInstance.SaveChanges();
            Debug.Log("AutoSave");
        }
    }

    private void CenterCanvasOnNodes()
    {
        if (jNodeInstance != null && Nodes.Count > 0)
        {
            Vector2 sumPositions = Vector2.zero;
            foreach (Node node in Nodes)
            {
                sumPositions += node.Rect.center;
            }
            Vector2 averageCenter = sumPositions / Nodes.Count;
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
         
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            node.DrawNode(CanvasOffset);  // 각 노드를 그림
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];

            // Iterate over each ChildConnectingPoint in the list
            ConnectingPoint connectingPoint = node.ChildConnectingPoint;
            if (connectingPoint.isConnected)
            {
                float lineThickness = 5.0f;
                // Assume GetNode returns a Node object based on an ID and it handles null cases appropriately
                Node connectedParentNode = GetNode(connectingPoint.ConnectedNodeId);
                if (connectedParentNode != null)
                {
                    Handles.DrawAAPolyLine(lineThickness, new Vector3[]
                        {
                        connectingPoint.rect.center,
                        connectedParentNode.ParentConnectingPoint.rect.center
                        });
                }
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
        string currentSnapshot = JsonConvert.SerializeObject(jNodeInstance, new JsonSerializerSettings
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
        if (Nodes.Count > 0)
        {
            string resourcesPath = StoragePath.ScenarioPath;

            string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

            string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", initialDirectory, "TestJson", "json");

            if (!string.IsNullOrEmpty(path2))
            {
                Node startNode = Nodes[0];
                if (GetNode(startNode.ChildConnectingPoint.connectedNodeId) == null)
                {
                    EditorUtility.DisplayDialog("Missing Connection",
                                                              "The Start Node must be connected. Please check your nodes.",
                                                              "OK"); return; 
                }

                Node parentNode = startNode;
                List<Node> connectedNode = new List<Node>();
                while (true)
                {
                    Node childNode = GetNode(parentNode.ChildConnectingPoint.connectedNodeId);

                    if (childNode == null)
                    {
                        break;
                    }
                    else
                    {
                        connectedNode.Add(childNode);
                        parentNode = childNode;
                    }
                }

                List<Element> elements = connectedNode.ToElements();
                Debug.Log(elements.Count);
                Scenario scenario = new Scenario(elements);
                for (int i = 0; i < elements.Count; i++)
                {
                    Debug.Log(elements[i]);
                }
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
        lastSavedSnapshot = JsonConvert.SerializeObject(jNodeInstance, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new Vector2Converter() },
            StringEscapeHandling = StringEscapeHandling.Default
        });
    }
    public static void SaveJNode()
    {
        string currentSnapshot = JsonConvert.SerializeObject(jNodeInstance, new JsonSerializerSettings
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
        JNode jNode = new JNode(jNodeInstance.Nodes);
        jNode.editorUIState = jNodeInstance.editorUIState;
        ArokaJsonUtils.SaveJNode(jNode, path);
        UpdateLastSavedSnapshot();
        Debug.Log($"<color=green>Save Complete</color> " + RecentOpenFileName);

    }


    public void EditorControl(Event e)
    {
        MousePosition = (e.mousePosition - CanvasOffset);

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F1)
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

                    foreach (var node in Nodes)
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
                    SelectedNode.UpdateNodePosition(SelectedNode.position + e.mousePosition - LastMouseDragPosition);
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
                    foreach (var node in Nodes)
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
                        Debug.Log("De Connect");
                        ConnectStartNode.DeConnectNodeChild();
                    }
                }
                ConnectStartNode = null;
                e.Use();
                break;

            case EventType.KeyDown:
                bool isDeleteCommand = (e.keyCode == KeyCode.Delete && Application.platform != RuntimePlatform.OSXEditor) ||
                             (e.command && e.keyCode == KeyCode.Backspace && Application.platform == RuntimePlatform.OSXEditor);

                if (isDeleteCommand && SelectedNode != null)
                {
                    Debug.Log("Destory Node " + SelectedNode + " | " + Nodes.Count );
                    
                    if (GetNodeByChild(SelectedNode.ID) != null)
                        GetNodeByChild(SelectedNode.ID).DeConnectNodeChild();
                    if (GetNodeByParent(SelectedNode.ID) != null)
                        GetNodeByParent(SelectedNode.ID).DeConnectNodeParent();

                    SelectedNode.DeConnectNodeChild();
                    SelectedNode.DeConnectNodeParent();
                    Nodes.Remove(SelectedNode);
                    Debug.Log("Destory Node " + SelectedNode + " | " + Nodes.Count);

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
        menu.AddItem(new GUIContent("Add ItemModify Node"), false, () => AddItemModifyNode(mousePos)); // Changed from AssetChange
        menu.AddItem(new GUIContent("Add ItemDemand Node"), false, () => AddItemDemandNode(mousePos));
        menu.AddItem(new GUIContent("Add PositionInit Node"), false, () => AddPositionInitNode(mousePos));
        menu.AddItem(new GUIContent("Add PlaceModify Node"), false, () => AddPlaceModifyNode(mousePos)); // Newly added
        menu.AddItem(new GUIContent("Add FriendshipModify Node"), false, () => AddFriendshipModifyNode(mousePos)); // Newly added
        menu.AddItem(new GUIContent("Add OverlayPicture Node"), false, () => AddOverlayPictureNode(mousePos)); // Newly added


        menu.ShowAsContext();
    }
    private void AddDialogueNode(Vector2 position)
    {
        DialogueNode node = new DialogueNode(position, "Dialogue");
        node.SetGuid();
        node.dialogue = new Dialogue("Mono", new List<Line>() { });
        Nodes.Add(node);
        AutoSaveJNodeInstance();
    }


    private void AddChoiceSetNode(Vector2 position)
    {
        ChoiceSetNode node = new ChoiceSetNode(position, "Choice Set");
        node.SetGuid();
        Nodes.Add(node);
    }

    private void AddPlaceModifyNode(Vector2 position)
    {
        PlaceModifyNode node = new PlaceModifyNode(position, "PlaceModifyNode");
        node.SetGuid();  // Assuming SetGuid sets a unique identifier for the node.
        Nodes.Add(node);  // Assuming Nodes is a list or collection that stores all nodes.
    }

    private void AddFriendshipModifyNode(Vector2 position)
    {
        FriendshipModifyNode node = new FriendshipModifyNode(position, "FriendshipModifyNode");
        node.SetGuid();  // Ensure each node has a unique ID for reference.
        Nodes.Add(node);  // Add this node to your node management system or editor.
    }

    private void AddOverlayPictureNode(Vector2 position)
    {
        OverlayPictureNode node = new OverlayPictureNode(position, "OverlayPictureNode");
        node.SetGuid();  // Unique GUID for the node.
        Nodes.Add(node);  // Append to the list of nodes.
    }

    private void AddItemModifyNode(Vector2 position)
    {
        ItemModifyNode node = new ItemModifyNode(position, "ItemModifyNode");
        node.SetGuid();
        Nodes.Add(node);
    }

    private void AddItemDemandNode(Vector2 position)
    {
        ItemDemandNode node = new ItemDemandNode(position, "Item Demand");
        node.SetGuid();
        Nodes.Add(node);
    }

    private void AddPositionInitNode(Vector2 position)
    {
        PositionInitNode node = new PositionInitNode(position, "Position Init");
        node.SetGuid();
        Nodes.Add(node);
    }

    public Node GetNode(string id)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];

            if (node.ID == id)
            {
                return node; // Node found
            }
        }
        return null; // No node found with the given ID
    }
    
    public Node GetNodeByChild(string id)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            
            if (node.ChildConnectingPoint.connectedNodeId == id)
            {
                return node; // Node found
            }
        }
        return null; // No node found with the given ID
    }
    public Node GetNodeByParent(string id)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];

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
    void OnLostFocus()
    {
        jNodeInstance.SaveChanges();
        Debug.Log("JNode Editor lost focus");
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
