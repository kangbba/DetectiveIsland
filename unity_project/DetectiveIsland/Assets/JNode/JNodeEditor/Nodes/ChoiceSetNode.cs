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
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 0; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public const float DEFAULT_WIDTH = 600;
    public List<DialogueNode> DialogueNodes = new List<DialogueNode>();
    public List<ChoiceNode> ChoiceNodes = new List<ChoiceNode>();



    public override float Width { get; set; }
    public override float Height { get; set; }

    private float Size_EndOfAddDialoguesBtn => UPPER_MARGIN + DialogueNodes.Cast<Node>().GetNodesHeight() + AddDialogueBtn_UpperMargin;


    private float AddDialogueBtn_UpperMargin = 50;
    private float AddDialogueBtn_BottomMargin = 70;

    private float AddChoiceBtn_UpperMargin = 50;
    private float AddChoiceBtn_BottomMargin = 50;


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
    public ChoiceSetNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public const float dialoguesDist = 10;


    public override void DrawNode()
    {
        base.DrawNode();
        Width = 0;
        Height = UPPER_MARGIN;
        for (int i = 0; i < DialogueNodes.Count; i++)
        {
            Node node = DialogueNodes[i];
            float xPos = NodeRect.position.x + GetCenterLocalPosX(node, this);
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            Height += node.Height;
        }

        Height += AddDialogueBtn_UpperMargin;

        JButton addDialogueButton = new JButton
        (
            pos : new Vector2(NodeRect.center.x, NodeRect.position.y + Height),
            size : new Vector2(80, 30),
            title : "Add Dialogue",
            action : AddDialogue, 
            anchor : JAnchor.Center
        );
        addDialogueButton.Draw();
        Height += AddDialogueBtn_BottomMargin;
        for (int i = 0; i < ChoiceNodes.Count; i++)
        {
            Node node = ChoiceNodes[i];
            node.DrawNode();
            Vector2 pos = NodeRect.position + new Vector2(i * node.Width, Height);
            node.SetRectPos(pos);
            Width += node.Width;
        }
        Height += ChoiceNodes.GetMaxHeight();
        Height += AddChoiceBtn_UpperMargin;
        JButton addChoiceButton = new JButton(
            pos : new Vector2(NodeRect.center.x, NodeRect.position.y + Height),
            size :  new Vector2(80, 30),
            title : "Add Choice",
            action : AddChoice,  
            anchor : JAnchor.Center);
            addChoiceButton.Draw();
        Height += AddChoiceBtn_BottomMargin;

        Height += BOTTOM_MARGIN;
        SetNodeRectSize(ChoiceNodes.Count == 0 ? 500 : Width, Height);
    }

    private void AddDialogue()
    {
        DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
        DialogueNodes.Add(dialogueNode);
        Debug.Log("Dialogue node added.");
    }

    private void AddChoice()
    {
        ChoiceNode choiceNode = new ChoiceNode(Guid.NewGuid().ToString(), "ChoiceNode", NodeID);
        ChoiceNodes.Add(choiceNode);
        Debug.Log("Choice node added.");
    }


    private float GetCenterLocalPosX(Node node, Node parentNode)
    {
        return parentNode.NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
    }





}