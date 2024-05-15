using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEditor;
using UnityEngine;




[System.Serializable]
public class PositionInitNode : Node
{   
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width => 300;

    public override float Height => 200;

    private PositionInit _positionInit = new PositionInit(new List<CharacterPosition>{new CharacterPosition("", "Middle")});
    private List<PositionInitNode> positionInitNodes = new List<PositionInitNode>();

    public PositionInitNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return _positionInit;
    }

    public void AddCharacterPosition()
    {

    }
    public override void DrawNode()
    {
        base.DrawNode();
        for(int i = 0 ; i < positionInitNodes.Count ; i++){
            positionInitNodes[i].DrawNode();
        }
    }
}

    