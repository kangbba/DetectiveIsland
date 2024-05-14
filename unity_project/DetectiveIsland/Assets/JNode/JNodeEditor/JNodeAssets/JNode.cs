using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class JNode
{
    private List<Node> nodes = new List<Node>();

    public List<Node> Nodes
    {
        get => nodes;
        set
        {
            nodes = value;

            JNodeInstance jNodeInstance = AssetDatabase.LoadAssetAtPath<JNodeInstance>("Assets/JNode/JNodeInstance.asset");

            // Assuming you have a reference to the JNodeInstance that contains this JNode
            EditorUtility.SetDirty(jNodeInstance); // Mark the JNodeInstance as dirty
        }
    }
    public EditorUIState editorUIState;
    public JNode(List<Node> nodes)
    {
        this.nodes = nodes;
        editorUIState = new EditorUIState(); 
    }
}


[System.Serializable]
public class EditorUIState
{
    // Transient UI states not serialized
    [System.NonSerialized] public Vector2 mousePosition;
    [System.NonSerialized] public Vector2 lastMouseDragPosition;
    [System.NonSerialized] public Vector2 canvasOffset;
    [System.NonSerialized] public bool isDraggingNode;
    [System.NonSerialized] public bool isPanningCanvas;

    // Node interaction states
    public Node selectedNode;
    public Node connectStartNode;

    public EditorUIState()
    {
        ResetUIState();
        ResetNodeState();
    }

    // Reset UI-specific states to default values
    public void ResetUIState()
    {
        mousePosition = Vector2.zero;
        lastMouseDragPosition = Vector2.zero;
        canvasOffset = Vector2.zero;
        isDraggingNode = false;
        isPanningCanvas = false;
    }

    // Reset node interaction states
    public void ResetNodeState()
    {
        selectedNode = null;
        connectStartNode = null;
    }
}
