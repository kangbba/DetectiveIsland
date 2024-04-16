using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NodeEditor : EditorWindow
{
    private List<Node> nodes;
    private Node selectedNode;
    private Vector2 mousePosition;
    private Vector2 lastMouseDragPosition; // Track the last position of the mouse during a drag
    private Vector2 canvasOffset;
    private bool isDraggingNode;
    private bool isPanning;

    [MenuItem("JNode/Create Json Node")]
    private static void OpenWindow()
    {
        NodeEditor window = GetWindow<NodeEditor>();
        window.titleContent = new GUIContent("Node Editor");
    }

    private void OnEnable()
    {
        nodes = new List<Node>();
        canvasOffset = Vector2.zero;
    }

    private void OnGUI()
    {
        ProcessEvents(Event.current);
        DrawNodes();

        if (isDraggingNode && selectedNode != null)
        {
            selectedNode.rect.position = mousePosition - selectedNode.dragOffset + canvasOffset;
            Repaint();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Nodes to JSON"))
        {
            SaveCurrentNodes();
        }
        GUILayout.EndHorizontal();
    }

    private void ProcessEvents(Event e)
    {
        mousePosition = e.mousePosition;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 2) // Middle mouse button
                {
                    isPanning = true;
                    lastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (e.button == 1) // Right click
                {
                    ProcessContextMenu();
                    e.Use();
                }
                else if (e.button == 0) // Left click
                {
                    Node node = GetNodeAtPosition(mousePosition - canvasOffset);
                    if (node != null)
                    {
                        selectedNode = node;
                        if (e.clickCount == 2) // Double click
                        {
                            e.Use();
                        }
                        else
                        {
                            isDraggingNode = true;
                            selectedNode.dragOffset = mousePosition - selectedNode.rect.position;
                            e.Use();
                        }
                    }
                    else
                    {
                        selectedNode = null; // Clear selection if click outside any node
                        isDraggingNode = false;
                    }
                }
                break;
            case EventType.MouseDrag:
                if (isPanning && e.button == 2)
                {
                    Vector2 delta = e.mousePosition - lastMouseDragPosition;
                    canvasOffset += delta;
                    lastMouseDragPosition = e.mousePosition;
                    Repaint();
                }
                break;
            case EventType.MouseUp:
                if (e.button == 2)
                    isPanning = false;
                isDraggingNode = false;
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && selectedNode != null)
                {
                    nodes.Remove(selectedNode);
                    selectedNode = null;
                    Repaint();
                }
                break;
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Line Node"), false, () => OnClickAddNode(mousePosition - canvasOffset));
        if (selectedNode != null)
        {
            genericMenu.AddItem(new GUIContent("Delete Node"), false, () => DeleteSelectedNode());
        }
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position)
    {
        LineNode newNode = new LineNode(position, 200, 100, "New Line Node");
        nodes.Add(newNode);
    }

    private void DeleteSelectedNode()
    {
        if (selectedNode != null)
        {
            nodes.Remove(selectedNode);
            selectedNode = null;
            Repaint();
        }
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            Rect adjustedRect = new Rect(node.rect.position + canvasOffset, node.rect.size);
            node.rect = adjustedRect;
            node.DrawNode();
            node.rect.position -= canvasOffset; // Reset the node's rect for consistent logical operations
        }
        if (selectedNode != null)
        {
            // Highlight the selected node
            GUI.color = Color.cyan;
            GUI.Box(selectedNode.rect, "", EditorStyles.helpBox);
            GUI.color = Color.white;
        }
    } 



    private Node GetNodeAtPosition(Vector2 position)
    {
        foreach (var node in nodes)
        {
            if (node.rect.Contains(position))
            {
                return node;
            }
        }
        return null;
    }

    private void SaveCurrentNodes()
    {
        if (nodes.Count > 0)
        {
            JsonUtilityHelper.SaveNodesToJson(nodes, "NodesData.json");
            Debug.Log("Nodes saved to JSON.");
        }
        else
        {
            Debug.Log("No nodes to save.");
        }
    }
}
public static class JsonUtilityHelper
{
    public static void SaveNodesToJson(List<Node> nodes, string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);
        string json = JsonUtility.ToJson(new Serialization<Node>(nodes), true);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
    }

    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> items;
        public Serialization(List<T> items)
        {
            this.items = items;
        }
    }
}