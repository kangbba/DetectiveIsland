using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class ChoiceSetNode : Node
{
    private List<DialogueNode> _dialogueNodes = new List<DialogueNode>();
    private List<ChoiceNode> _choiceNodes = new List<ChoiceNode>();


    public void AddChoicePlan()
    {
         
    }

    public override Element ToElement()
    {
        return new ChoiceSet(null, null);
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

        for(int i = 0 ; i < _dialogueNodes.Count ; i++){
            _dialogueNodes[i].DrawNode();
        }
        for(int i = 0 ; i < _choiceNodes.Count ; i++){
            _choiceNodes[i].DrawNode();
        }
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColors.choiceSetColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColors.choiceSetColor);
    }


}