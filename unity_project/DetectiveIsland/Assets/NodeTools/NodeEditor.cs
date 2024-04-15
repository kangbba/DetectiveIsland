using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeEditor : EditorWindow
{
    private List<Node> nodes;
    private Node selectedNode;
    private Vector2 mousePosition;
    private bool isDraggingNode;

    [MenuItem("Window/Custom Node Editor")]
    private static void OpenWindow()
    {
        NodeEditor window = GetWindow<NodeEditor>();
        window.titleContent = new GUIContent("Node Editor");
    }

    private void OnEnable()
    {
        nodes = new List<Node>();
    }

    private void OnGUI()
    {
        ProcessEvents(Event.current);

        DrawNodes();

        if (isDraggingNode && selectedNode != null)
        {
            selectedNode.rect.position = mousePosition - selectedNode.dragOffset;
            Repaint();
        }
    }

    private void ProcessEvents(Event e)
    {
        mousePosition = e.mousePosition;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1) // Right click
                {
                    ProcessContextMenu();
                    e.Use();
                }
                else if (e.button == 0) // Left click
                {
                    selectedNode = GetNodeAtPosition(mousePosition);
                    if (selectedNode != null)
                    {
                        if (e.clickCount == 2) // Double click
                        {
                            SetNodeTitle(selectedNode);
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
                        isDraggingNode = false;
                    }
                }
                break;
            case EventType.MouseUp:
                isDraggingNode = false;
                break;
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Line Node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position)
    {
        LineNode newNode = new LineNode(position, 200, 100, "New Line Node");
        newNode.emotionID = "Unknown"; // default value
        newNode.sentence = "Type sentence here"; // default placeholder
        nodes.Add(newNode);
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            node.DrawNode();
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

    private void SetNodeTitle(Node node)
    {
        string newTitle = EditorGUILayout.TextField("Node Title", node.title);
        if (newTitle != node.title)
        {
            node.title = newTitle;
        }
    }
}
