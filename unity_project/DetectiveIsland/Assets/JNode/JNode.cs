using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JNode
{
    [SerializeField]
    [SerializeReference]
    private List<Node> nodes = new List<Node>();

    public List<Node> Nodes
    {
        get => nodes;
        set
        {
            nodes = value;
        } 
    }

    public JNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }
}