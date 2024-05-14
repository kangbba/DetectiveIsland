using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEditor;
using UnityEngine;




[System.Serializable]
public class PositionInitNode : Node
{   
    private PositionInit _positionInit = new PositionInit(new List<CharacterPosition>{new CharacterPosition("", "Middle")});
    private List<PositionInitNode> positionInitNodes = new List<PositionInitNode>();

    public PositionInitNode(string title, Node parentNode) : base(title, parentNode)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return _positionInit;
    }

    public void AddCharacterPosition()
    {

    }
    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        for(int i = 0 ; i < positionInitNodes.Count ; i++){
            positionInitNodes[i].DrawNode();
        }
        
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColors.positionInitColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColors.positionInitColor);
    }
}

    