using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JNode
{
    private List<Node> nodes;

    public JNode(List<Node> nodes)
    {


        this.nodes = nodes;
    }

    public List<Node> Nodes { get => nodes;}
}
