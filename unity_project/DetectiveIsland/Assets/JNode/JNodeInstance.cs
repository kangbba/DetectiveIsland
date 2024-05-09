using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public  class JNodeInstance : ScriptableObject
{
    public JNode jNode;
    private List<Node> nodes = new List<Node>();
    public List<Node> Nodes
    {
        get => nodes;
        set
        {
            nodes = value;
            // Assuming you have a reference to the JNodeInstance that contains this JNode
            EditorUtility.SetDirty(this); // Mark the JNodeInstance as dirty
        }
    }
    public string recentOpenFileName;
    public string recentPath;
    public EditorUIState editorUIState;

    public void Initialize(string recentPath, string _recentOpenFileName, JNode jNode)
    { 
        Debug.Log("Jnode Instance Initialize");
        this.jNode = jNode;
        this.recentPath = recentPath;
        nodes = jNode.Nodes;
        editorUIState = jNode.editorUIState;
        recentOpenFileName = _recentOpenFileName;
        editorUIState = jNode.editorUIState;
    }

    public void SaveEditorUiState(ref JNode jNode)
    {
        jNode.editorUIState = editorUIState;
    }


    public void SaveChanges()
    {
        EditorUtility.SetDirty(this); // Mark the ScriptableObject as dirty to ensure it gets saved
        AssetDatabase.SaveAssets(); // 변경 사항을 디스크에 저장
        
    }
}
