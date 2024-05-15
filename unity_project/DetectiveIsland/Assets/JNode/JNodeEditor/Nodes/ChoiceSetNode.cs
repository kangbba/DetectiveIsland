using Aroka.ArokaUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class ChoiceSetNode : Node
{
    Vector2 AddDialogueBtnPos => NodeRect.position  + new Vector2(0,100) + new Vector2(NodeRect.width, DialougeTotalHeight); //new Vector2(NodeRect.max.x, (DialogueNodes != null && DialogueNodes.Count > 0) ? DialogueNodes.Last().NodeRect.position.y : 100) + NodeRect.position;
    Vector2 AddChoiceSetBtnPos => AddDialogueBtnPos + new Vector2(0,200); //new Vector2(NodeRect.max.x, (ChoiceNodes != null && ChoiceNodes.Count > 0) ? ChoiceNodes.Last().NodeRect.position.y : 100) + NodeRect.position;
    float DialougeTotalHeight => (DialogueNodes != null && DialogueNodes.Count > 0) ? NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y : 0;

    public const float ChoiceNodeSpacingX = 30;
    
    public List<DialogueNode> DialogueNodes = new List<DialogueNode>();
    public List<ChoiceNode> ChoiceNodes = new List<ChoiceNode>();


    public override Element ToElement()
    {
        ChoiceSet choiceSet = new ChoiceSet(null,null);
        choiceSet.Dialogues = new List<Dialogue>();
        for (int i = 0; i < DialogueNodes.Count; i++)
        {
            choiceSet.Dialogues.Add(DialogueNodes[i].ToElement() as Dialogue);
        }
        choiceSet.Choices = new List<Choice>();
        for (int i = 0; i < ChoiceNodes.Count; i++)
        {
            Choice choice = new Choice(ChoiceNodes[i].Content,null);
            for (int j = 0; j < ChoiceNodes[i].Nodes.Count; j++)
            {
                choice.Elements.Add(ChoiceNodes[i].Nodes[j].ToElement());
            }
            choiceSet.Choices.Add(choice);
        }
        return choiceSet;
    }

    public override Vector2 CalNodeSize()
    {
        float choiceNodesTotalWidth =( ChoiceNodes != null && ChoiceNodes.Count > 0) ? NodeService.CalNodesSize(ChoiceNodes.Cast<Node>().ToList()).x : 500;
        float width = choiceNodesTotalWidth + ChoiceNodes.Count * ChoiceNodeSpacingX;

        float dialogueTotalSizeY = NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;
        float longestChoiceHeight =  ChoiceNodes.Any() ? ChoiceNodes.Max(node => node.ChoicesTotalSizeY) : 0;
        float height = dialogueTotalSizeY + longestChoiceHeight;
        return new Vector2(width, height + 500);
    }
    public ChoiceSetNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        RefreshNodeSize();
    }

    public const float dialoguesDist = 10;

    public override void DrawNode()
    {
        base.DrawNode();

        float accumulatedHeight = 0;
        
        for(int i = 0 ; i < DialogueNodes.Count ; i++){
            Node node = DialogueNodes[i];
            float xPos = NodeRect.position.x + GetCenterLocalPosX(node,this);
            float yPos = NodeRect.position.y + 100 + accumulatedHeight;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            accumulatedHeight += node.CalNodeSize().y + dialoguesDist;
        }
        
        for(int i = 0 ; i < ChoiceNodes.Count ; i++){
            Node node = ChoiceNodes[i];
            node.DrawNode();
            Vector2 pos = NodeRect.position + new Vector2( i * (ChoiceNode.ChoiceNodeWidth + ChoiceNodeSpacingX), DialougeTotalHeight + 200);
            node.SetRectPos(pos);
        }

        DrawAddDialogueButton();
        DrawChoiceButton();
        RefreshNodeSize();


    }
    private float GetCenterLocalPosX(Node node, Node parentNode)
    {
        return parentNode.NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
    }
    private void DrawAddDialogueButton()
    {
        float y = NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;
        Rect buttonRect = new Rect(AddDialogueBtnPos.x, AddDialogueBtnPos.y, 100, 60); // Position below the node
        if (GUI.Button(buttonRect, "Add Dialogue"))
        {
            DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
            DialogueNodes.Add(dialogueNode);
        }
    }

    private void DrawChoiceButton()
    {
        float y = NodeService.CalNodesSize(ChoiceNodes.Cast<Node>().ToList()).y;
        Rect buttonRect = new Rect(AddChoiceSetBtnPos.x, AddChoiceSetBtnPos.y, 100, 60); // Position below the node
        if (GUI.Button(buttonRect, "Add Choice"))
        {
            ChoiceNode choiceNode = new ChoiceNode(Guid.NewGuid().ToString(), "ChoiceNode", NodeID);
            ChoiceNodes.Add(choiceNode);
        }
    }





}