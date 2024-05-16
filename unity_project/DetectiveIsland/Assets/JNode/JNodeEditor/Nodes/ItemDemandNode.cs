using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Aroka.ArokaUtils;


[System.Serializable]
public class ItemDemandNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    private string _itemID;
    private List<DialogueNode> _dialogueNodes;
    private List<Node> _successNodes;
    private List<Node> _failNodes;
    private List<Node> _cancelNodes;
    public override float Width { get; set; }
    public override float Height { get; set; }


    public ItemDemandNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return new ItemDemand(null, null, null, null);
    }

    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;

        _itemID = (string)JInterface.SimpleField
        (
            value : _itemID,
            pos : new Vector2(NodeRect.position.x, Height),
            title: "Item ID : ",
            labelWidth : 100,
            fieldWidth : 20,
            fieldHeight : 20
        );

        
        if(_dialogueNodes != null)
        {
            foreach(DialogueNode node in _dialogueNodes){
                Debug.Log("여기실행");
                node.DrawNode();
            }
        }
        if(_successNodes != null)
        {
            foreach(Node node in _successNodes){
                node.DrawNode();
            }
        }
        if(_failNodes != null)
        {
            foreach(Node node in _failNodes){
                node.DrawNode();
            }
        }
        if(_cancelNodes != null)
        {
            foreach(Node node in _cancelNodes){
                node.DrawNode();
            }
        }

        Height += BOTTOM_MARGIN;
    }
}
