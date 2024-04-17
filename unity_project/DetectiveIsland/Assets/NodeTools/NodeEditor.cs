using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;


public class NodeEditor : EditorWindow
{
    private List<Node> nodes = new List<Node>();
    private Node selectedNode;
    private Vector2 mousePosition;
    private Vector2 lastMouseDragPosition;
    private Vector2 canvasOffset;
    private bool isDraggingNode;
    private bool isPanningCanvas;

    [MenuItem("JNode/Create Json Node")]
    private static void OpenWindow()
    {
        NodeEditor window = GetWindow<NodeEditor>("JNode Editor");
        window.Show();
    }

    private void OnGUI()
    {
        ProcessEvents(Event.current);
        DrawGrid();
        DrawNodes();
        if (GUI.changed) Repaint();
    }

    private void DrawGrid()
    {
        float gridSize = 20f;
        float gridOpacity = 0.2f;
        int widthDivs = Mathf.CeilToInt(position.width / gridSize);
        int heightDivs = Mathf.CeilToInt(position.height / gridSize);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, gridOpacity);

        for (int i = 0; i <= widthDivs; i++)
        {
            Handles.DrawLine(new Vector2(gridSize * i, 0) + canvasOffset, new Vector2(gridSize * i, position.height) + canvasOffset);
        }
        for (int j = 0; j <= heightDivs; j++)
        {
            Handles.DrawLine(new Vector2(0, gridSize * j) + canvasOffset, new Vector2(position.width, gridSize * j) + canvasOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            node.DrawNode(canvasOffset);
        }
    }

    private void ProcessEvents(Event e)
    {
        mousePosition = e.mousePosition - canvasOffset;

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
                    selectedNode = null;
                    foreach (var node in nodes)
                    {
                        if (node.rect.Contains(mousePosition))
                        {
                            selectedNode = node;
                            isDraggingNode = true;
                            lastMouseDragPosition = e.mousePosition;
                            break;
                        }
                    }

                    if (selectedNode == null)
                    {
                        isPanningCanvas = true;
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
                    nodes.Remove(selectedNode);
                    selectedNode = null;
                    e.Use();
                }
                break;
        }
    }

    private void ProcessContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, () => AddNode(mousePos));
        menu.ShowAsContext();
    }

    private void AddNode(Vector2 position)
    {
        nodes.Add(new DialogueNode(new Rect(position.x, position.y, 200, 100), "Dialogue"));
    }
}