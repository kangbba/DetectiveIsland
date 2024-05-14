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

    private Vector2 _lastMousePositionDrag;
    private Vector2 _dragOffset;
    private Vector2 _canvasOffset;
    private bool _isCanvasPanning;
    private bool _isNodeDragging;
    private ConnectingPoint _startingCPoint;
    
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

    
    [DidReloadScripts]
    public static void ReloadEditor()
    {
        Debug.Log("Reloaded");
        LoadJNodeInstance(); 
           
        LoadJNodeEditorWindow(jNodeInstance.recentPath, RecentOpenFileName);
        UpdateLastSavedSnapshot(); 
    }

    private void OnGUI()
    {
      //  DrawGrid();
        DrawJNodeMenuBar();
        EditorControl(Event.current);
        if (jNodeInstance != null)
        {
            DrawNodes();
            DrawConnectingPointLines();
            ProcessEvents(Event.current); 
        }
        DrawJNodeMenuBar();
        if (GUI.changed) Repaint();
        AutoSaveJNodeInstance();

    }
    public void Connect(string fromNodeID, string toNodeID)
    {
        GetNode(fromNodeID).SetNextNodeID(toNodeID);
    }

    public void DeConnect(string fromNodeID)
    {
        GetNode(fromNodeID).SetNextNodeID("");
    }

     private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                // 노드 선택 로직
                if (e.button == 0) // 왼쪽 마우스 버튼
                {  
                    ConnectingPoint connectingPoint = GetMouseOverConnectingPoint(e.mousePosition);
                    _startingCPoint = connectingPoint;
                    
                    Node node = GetMouseOverNode(e.mousePosition);
                    if(node != null){
                        node.SetSelected(true);
                        SelectedNode = node;
                    }
                    else{
                        SelectedNode = null;
                    }
                    if (SelectedNode != null)
                    {
                        if(SelectedNode.IsMouseOver(e.mousePosition)){
                            _isNodeDragging = true;
                            _lastMousePositionDrag = e.mousePosition;
                        }
                    } 
                }
                else if (e.button == 1) // Right click for context menu
                {
                    ProcessContextMenu(e.mousePosition);
                }
                else if (e.button == 2) // Middle mouse button for panning
                {
                    _isCanvasPanning = true;
                    _lastMousePositionDrag = e.mousePosition;
                }
                e.Use();
                Debug.Log(SelectedNode);
                break;

            case EventType.MouseUp:
                // // 드래그 종료
                _isCanvasPanning = false;
                _isNodeDragging = false;

                ConnectingPoint endingCPoint = GetMouseOverConnectingPoint(e.mousePosition);
                if(_startingCPoint != null && endingCPoint != null && _startingCPoint != endingCPoint){
                    
                    bool isStartingCPointChild = _startingCPoint.IsChildConnectingPoint; // 시작 코넥팅 포인트가 자식이니?
                    bool isEndingCPointChild = endingCPoint.IsChildConnectingPoint; // 끝 코넥팅 포인트가 자식이니?
                    
                    if(!isStartingCPointChild && isEndingCPointChild){ // 시작이 부모었고, 끝이 자식이란 뜻
                        string startingNodeID = _startingCPoint.NodeID;
                        string endingNodeID = endingCPoint.NodeID;
                        Debug.Log($"안정상, 역순연결 {startingNodeID} {endingNodeID}");
                        Connect(endingNodeID, startingNodeID);
                    }
                    else if(isStartingCPointChild && !isEndingCPointChild){ // 시작이 자식이었고, 끝이 부모란 뜻
                        string startingNodeID = _startingCPoint.NodeID;
                        string endingNodeID = endingCPoint.NodeID;
                        Debug.Log($"정상, 순방향연결 {startingNodeID} {endingNodeID}");
                        Connect(startingNodeID, endingNodeID);
                    }
                    else{
                        _startingCPoint = null;
                    }
                }
                else if(_startingCPoint != null && endingCPoint == null){
                    string startingNodeID = _startingCPoint.NodeID;
                    if(_startingCPoint.IsChildConnectingPoint){
                        DeConnect(startingNodeID);
                    }
                    Debug.Log($"ELSE22");
                    _startingCPoint = null;
                }
                else{
                    Debug.Log($"ELSE33");
                }
                e.Use();
                break;

            case EventType.MouseDrag:
                // 노드 드래그 로직
                if (_isNodeDragging)
                {
                    Vector2 delta = e.mousePosition - _lastMousePositionDrag;
                    _lastMousePositionDrag = e.mousePosition;
                    _dragOffset = delta;
                    SelectedNode.SetRectPos(SelectedNode.NodeRect.position + _dragOffset);
                }
                else if (_isCanvasPanning)
                {
                    Vector2 delta = e.mousePosition - _lastMousePositionDrag;
                    _lastMousePositionDrag = e.mousePosition;
                    _canvasOffset = delta;
                    SetNodeRectPoses(_canvasOffset);
                }
                e.Use();
                break;
        }
    }
    private Node GetMouseOverNode(Vector2 mousePos)
    {
        int cnt = Nodes.Count;
        for(int i = 0 ; i < cnt ; i++){
            Node node = Nodes[i];
            if(node.IsMouseOver(mousePos)){
                return node;
            }
        }
        return null;
    }
    private ConnectingPoint GetMouseOverConnectingPoint(Vector2 mousePos)
    {
        int cnt = Nodes.Count;
        for(int i = 0 ; i < cnt ; i++){
            Node node = Nodes[i];
            if(node.ParentConnectingPoint.IsContainRect(mousePos)){
                return node.ParentConnectingPoint;
            }
            if(node.ChildConnectingPoint.IsContainRect(mousePos)){
                return node.ChildConnectingPoint;
            }
        }
        return null;
    }
    public void SetNodeRectPoses(Vector2 offset){
        int count = Nodes.Count;
        for(int i = 0 ; i  < count ; i++){
            Node node = Nodes[i];
            node.SetRectPos(node.NodeRect.position + offset);
        }
    }
    public void AutoSaveJNodeInstance()
    {
          
        if (jNodeInstance != null && EditorUtility.IsDirty(jNodeInstance))
        {
            jNodeInstance.SaveChanges();
            Debug.Log("AutoSave");
        }
    }
    
    private void DrawNodes()
    {  
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            node.DrawNode();  // 각 노드를 그림
        }
    }

    private void DrawConnectingPointLines(){
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            if (node.IsNextNodeExist)
            {
                float lineThickness = 5.0f;
                // Assume GetNode returns a Node object based on an ID and it handles null cases appropriately
                Node nextNode = GetNode(node.NextNodeID);
                Handles.DrawAAPolyLine(lineThickness, new Vector3[]
                {
                    node.ChildConnectingPoint.Rect.center,
                    nextNode.ParentConnectingPoint.Rect.center
                });
            }
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
        Vector2 offset = new Vector2(_canvasOffset.x % gridSize, _canvasOffset.y % gridSize);

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
        // if (Nodes.Count > 0)
        // {
        //     string resourcesPath = StoragePath.ScenarioPath;

        //     string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

        //     string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", initialDirectory, "TestJson", "json");

        //     if (!string.IsNullOrEmpty(path2))
        //     {
        //         Node startNode = Nodes[0];
        //         if (GetNode(startNode.ChildConnectingPoint.connectedNodeId) == null)
        //         {
        //             EditorUtility.DisplayDialog("Missing Connection",
        //                                                       "The Start Node must be connected. Please check your nodes.",
        //                                                       "OK"); return; 
        //         }

        //         Node parentNode = startNode;
        //         List<Node> connectedNode = new List<Node>();
        //         while (true)
        //         {
        //             Node childNode = GetNode(parentNode.ChildConnectingPoint.connectedNodeId);

        //             if (childNode == null)
        //             {
        //                 break;
        //             }
        //             else
        //             {
        //                 connectedNode.Add(childNode);
        //                 parentNode = childNode;
        //             }
        //         }

        //         List<Element> elements = connectedNode.ToElements();
        //         Debug.Log(elements.Count);
        //         Scenario scenario = new Scenario(elements);
        //         for (int i = 0; i < elements.Count; i++)
        //         {
        //             Debug.Log(elements[i]);
        //         }
        //         // Save the scenario object as a JSON file at the specified path
        //         ArokaJsonUtils.SaveScenario(scenario, path2);
        //         Debug.Log("Nodes saved to JSON: " + path2);
        //     }
        // }
        // else
        // {
        //     Debug.Log("No nodes to save.");
        // }
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
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F1)
        {
         //   CenterCanvasOnNodes();
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
        DialogueNode node = new DialogueNode("DialogueNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);
        AutoSaveJNodeInstance();
    }


    private void AddChoiceSetNode(Vector2 position)
    {
        ChoiceSetNode node = new ChoiceSetNode("ChoiceSetNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);
    }

    private void AddPlaceModifyNode(Vector2 position)
    {
        PlaceModifyNode node = new PlaceModifyNode("PlaceModifyNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);  // Assuming Nodes is a list or collection that stores all nodes.
    }

    private void AddFriendshipModifyNode(Vector2 position)
    {
        FriendshipModifyNode node = new FriendshipModifyNode("FriendshipModifyNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);  // Add this node to your node management system or editor.
    }

    private void AddOverlayPictureNode(Vector2 position)
    {
        OverlayPictureNode node = new OverlayPictureNode("OverlayPictureNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);  // Append to the list of nodes.
    }

    private void AddItemModifyNode(Vector2 position)
    {
        ItemModifyNode node = new ItemModifyNode("ItemModifyNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);
    }

    private void AddItemDemandNode(Vector2 position)
    {
        ItemDemandNode node = new ItemDemandNode("ItemDemandNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);
    }

    private void AddPositionInitNode(Vector2 position)
    {
        PositionInitNode node = new PositionInitNode("PositionInitNode", null);
        node.SetRectPos(position);
        Nodes.Add(node);
    }

    public Node GetNode(string nodeID)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];

            if (node.ID == nodeID)
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

        StartNode startNode = new StartNode("StartNode", null);
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