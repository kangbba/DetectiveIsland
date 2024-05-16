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
    public JNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }
}

