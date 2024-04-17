using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ScenarioNode
{
    private List<Node> nodes;

    public ScenarioNode(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public List<Node> Nodes { get => nodes;}
}
