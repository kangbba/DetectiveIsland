using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class ChoiceSetNode : Node
{
    private ChoiceSet _choiceSet;
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>() {};


    public void AddChoicePlan()
    {
         
    }

    public override Element ToElement()
    {
        return _choiceSet;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }

    public ChoiceSetNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        SetNodeRectSize(CalNodeSize());

    }

    public const float dialoguesDist = 10;

    public override void DrawNode()
    {
        base.DrawNode();

        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.choiceSetColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.choiceSetColor);
    }


}