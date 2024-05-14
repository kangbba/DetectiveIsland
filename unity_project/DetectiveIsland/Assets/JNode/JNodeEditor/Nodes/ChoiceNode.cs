using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : Node
{
    private Choice _choice = new Choice("", new List<Element>());

    private List<Node> _nodes = new List<Node>();

    public ChoiceNode(string title, Node parentNode) : base(title, parentNode)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }

    public override Element ToElement()
    {
        return null;
    }

    public override void DrawNode()
    {
        base.DrawNode();
        _choice.Title = (string)CustomField("Title : ", _choice.Title, Vector2.down * 0f);
        for(int i = 0 ; i < _choice.Elements.Count ; i++){
            _nodes[i].DrawNode();
        }
    }



}
