using Aroka.JsonUtils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;


public class JNodeEditor : EditorWindow
{
    private static ScenarioNode scenarioNode;
    public static string recentOpenFileName;

    private Node selectedNode;
    private Vector2 mousePosition;
    private Vector2 lastMouseDragPosition;
    private Vector2 canvasOffset;
    private bool isDraggingNode;
    private bool isPanningCanvas;

    public static void OpenJNodeEditorWindow()
    {
        JNodeEditor window = GetWindow<JNodeEditor>("JNode Editor");
        window.Show();
    }

    public static void LoadJNodeEditor(string filePath, string _recentOpenFileName)
    {
        scenarioNode = ArokaJsonUtils.LoadScenarioNode(filePath);
        recentOpenFileName = _recentOpenFileName;
    }

    public void SaveScenarioNode()
    {
        string resourcesPath = StoragePath.ScenarioNodePath;

        // string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

        // Open the save file dialog with the determined initial directory
        string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", StoragePath.ScenarioNodePath, recentOpenFileName, "jnode");

        // Check if the user has not cancelled the operation
        if (!string.IsNullOrEmpty(path2))
        {
            List<Element> elements = scenarioNode.Nodes.ToElements();
            // Save the scenario object as a JSON file at the specified path
            ArokaJsonUtils.SaveScenarioNode(scenarioNode, path2);
            Debug.Log("Nodes saved to JSON: " + path2);
        }
    }

    private void ExportJson()
    {
        if (scenarioNode.Nodes.Count > 0)
        {

            string resourcesPath = StoragePath.ScenarioPath;

            string initialDirectory = Directory.Exists(resourcesPath) ? resourcesPath : Application.dataPath;

            // Open the save file dialog with the determined initial directory
            string path2 = EditorUtility.SaveFilePanel("Save Nodes as JSON", initialDirectory, "TestJson", "json");

            // Check if the user has not cancelled the operation
            if (!string.IsNullOrEmpty(path2))
            {
                List<Element> elements = scenarioNode.Nodes.ToElements();
                Debug.Log(elements.Count);
                Scenario scenario = new Scenario(elements);

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






    private void OnGUI()
    {
        DrawGrid();
        DrawJNodeMenuBar();

        if (scenarioNode != null)
        {
            DrawNodes();
            ProcessEvents(Event.current);
        }
      

        if (GUI.changed) Repaint();


    }




    public void DrawJNodeMenuBar()
    {

        Rect buttonArea = new Rect(10, 10, 250, 30);  // Increased width

        GUILayout.BeginArea(buttonArea);
        GUILayout.BeginHorizontal();

        // Set the color for the Save button
        GUI.color = Color.green;  // Green color for the Save button
        if (GUILayout.Button("Export", GUILayout.Width(90), GUILayout.Height(20)))  // Ensure the button has enough space
        {
            ExportJson();
        }

        GUI.color = Color.white;  // Green color for the Save button
        if (GUILayout.Button("Save JNode", GUILayout.Width(90), GUILayout.Height(20)))  // Ensure the button has enough space
        {
            SaveScenarioNode();
        }


        // Set the color for the label displaying the number of nodes
        GUI.color = Color.white; // Reset color to default

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
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
        Vector2 offset = new Vector2(canvasOffset.x % gridSize, canvasOffset.y % gridSize);

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
        foreach (var node in scenarioNode.Nodes)
        {
            node.DrawNode(canvasOffset);  // 각 노드를 그림
        }
    }




    private void ProcessEvents(Event e)
    {
        mousePosition = (e.mousePosition - canvasOffset) ;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 2) // Middle mouse button for panning
                {
                    isPanningCanvas = true;
                    lastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (e.button == 1) // Right click for context menu
                {
                    ProcessContextMenu(e.mousePosition);
                    e.Use();
                }
                else if (e.button == 0) // Left mouse button for selecting and dragging nodes
                {
                   if(selectedNode != null) selectedNode.Deselect();
                    selectedNode = null;

                    foreach (var node in scenarioNode.Nodes)
                    {
                        if (node.rect.Contains(mousePosition))
                        {
                            selectedNode = node;
                            Debug.Log(selectedNode);
                            selectedNode.Select();
                            isDraggingNode = true;
                            lastMouseDragPosition = e.mousePosition;
                            break;
                        }
                    }
                    isPanningCanvas = false;

                    if (selectedNode == null)
                    {
                        lastMouseDragPosition = e.mousePosition;
                    }
                    e.Use();
                }
                break;

            case EventType.MouseDrag:
                if (isPanningCanvas)
                {
                    canvasOffset += e.mousePosition - lastMouseDragPosition;
                    lastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (isDraggingNode && selectedNode != null)
                {
                    selectedNode.rect.position += e.mousePosition - lastMouseDragPosition;
                    lastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDraggingNode = false;
                isPanningCanvas = false;
                e.Use();
                break;

            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && selectedNode != null)
                {
                    scenarioNode.Nodes.Remove(selectedNode);
                    selectedNode = null;
                    e.Use();
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
        scenarioNode.Nodes.Add(dialogueNode);
    }
}