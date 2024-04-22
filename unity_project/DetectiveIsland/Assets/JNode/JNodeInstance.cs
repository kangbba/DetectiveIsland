using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public string recentPath;

    public void Initialize(string recentPath, string _recentOpenFileName, JNode jNode)
    {
        Debug.Log("Jnode Instance Initialize");
        this.jNode = jNode;
        this.recentPath = recentPath;
        selectedNode = null;
        canvasOffset = Vector2.zero;
        isDraggingNode = false;
        isPanningCanvas = false;
        recentOpenFileName = _recentOpenFileName;
    }


    public void SaveChanges()
    {
        EditorUtility.SetDirty(this); // Mark the ScriptableObject as dirty to ensure it gets saved
        AssetDatabase.SaveAssets(); // 변경 사항을 디스크에 저장
    }
}
