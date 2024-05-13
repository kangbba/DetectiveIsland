using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceNode : Node
{
    public Choice choice;

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

}
