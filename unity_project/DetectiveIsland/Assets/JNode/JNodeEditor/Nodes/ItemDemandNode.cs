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

    public ItemDemandNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
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
    }
}
