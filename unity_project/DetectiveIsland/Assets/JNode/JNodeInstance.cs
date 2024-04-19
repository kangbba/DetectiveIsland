using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public  class JNodeInstance : ScriptableObject
{
    public JNode jNode;
    public string recentOpenFileName;
    public Node selectedNode;
    public Vector2 mousePosition;
    public Vector2 lastMouseDragPosition;
    public Vector2 canvasOffset;
    public bool isDraggingNode;
    public bool isPanningCanvas;


    public void Initialize(string _recentOpenFileName, JNode jNode)
    {
        this.jNode = jNode;
        selectedNode = null;
        canvasOffset = Vector2.zero;
        isDraggingNode = false;
        isPanningCanvas = false;
        recentOpenFileName = _recentOpenFileName;
    }
}
