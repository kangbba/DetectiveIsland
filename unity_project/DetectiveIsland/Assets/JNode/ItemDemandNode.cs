using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class ItemDemandNode : Node
{
    public string itemId;
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>(); 
    public List<Node> successNodes = new List<Node>();
    public List<Node> failNodes = new List<Node>();
    public List<Node> cancelNodes = new List<Node>();

    public override Element ToElement()
    {
        List<Dialogue> dialogues = new List<Dialogue>();

        for (int i = 0; i < dialogueNodes.Count; i++)
        {
            dialogues.Add(dialogueNodes[i].ToElement() as Dialogue);
        }

        ItemDemand itemDemand = new ItemDemand(itemId, dialogues, successNodes.ToElements(), failNodes.ToElements());

        return itemDemand;
    }
    public ItemDemandNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(CalNodeSize());
        UpdateNodePosition(pos);
    }
    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }


    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.itemDemandColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);


        DrawConnectionPoints(representColor, true, true);
    }
}
