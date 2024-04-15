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
            // Update selected node's position
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
                if (e.button == 1)  // Right click
                {
                    ProcessContextMenu();
                    e.Use();
                }
                else if (e.button == 0)  // Left click
                {
                    selectedNode = GetNodeAtPosition(mousePosition);
                    if (selectedNode != null)
                    {
                        isDraggingNode = true;
                        selectedNode.dragOffset = mousePosition - selectedNode.rect.position;
                        e.Use();
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
        genericMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode());
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode()
    {
        nodes.Add(new Node(mousePosition, 200, 100, "New Node"));
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            GUI.Box(node.rect, node.title);
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
}
