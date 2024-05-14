using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Aroka.ArokaUtils;


[System.Serializable]
public class ItemDemandNode : Node
{
    private string _itemID;
    private List<DialogueNode> _dialogueNodes;
    private List<Node> _successNodes;
    private List<Node> _failNodes;
    private List<Node> _cancelNodes;

    public ItemDemandNode(string title, Node parentNode) : base(title, parentNode)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return new ItemDemand(null, null, null, null);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();

        _itemID = (string)CustomField("Item ID : ", _itemID, Vector2.up * 0f);
        
        foreach(DialogueNode node in _dialogueNodes){
            node.DrawNode();
        }
        foreach(Node node in _successNodes){
            node.DrawNode();
        }
        foreach(Node node in _failNodes){
            node.DrawNode();
        }
        foreach(Node node in _cancelNodes){
            node.DrawNode();
        }

        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColors.itemDemandColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColors.itemDemandColor);
    }
}
