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
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public List<DialogueNode> DialogueNodes = new List<DialogueNode>();
    public List<ChoiceNode> ChoiceNodes = new List<ChoiceNode>();


    public override float Width { get => 500; }
    public override float Height { get => UPPER_MARGIN + DialogueNodes.Cast<Node>().GetNodesHeight()+ ChoiceNodes.Cast<Node>().GetNodesHeight() + BOTTOM_MARGIN;  }


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

        float y = UPPER_MARGIN;
        
        for (int i = 0; i < DialogueNodes.Count; i++)
        {
            Node node = DialogueNodes[i];
            float xPos = NodeRect.position.x + GetCenterLocalPosX(node, this);
            float yPos = NodeRect.position.y + y;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            y += node.Height;
        }


        JButton addDialogueButton = new JButton
        (
            pos : new Vector2(NodeRect.max.x, NodeRect.position.y + y),
            size : new Vector2(80, 30),
            title : "Add Dialogue",
            action : AddDialogue, 
            anchor : JAnchor.CenterRight
        );
        addDialogueButton.Draw();


        for (int i = 0; i < ChoiceNodes.Count; i++)
        {
            Node node = ChoiceNodes[i];
            node.DrawNode();
            Vector2 pos = NodeRect.position + new Vector2(i * node.Width, node.Height);
            node.SetRectPos(pos);

            y += node.Height;
        }

        JButton addChoiceButton = new JButton(
            pos : new Vector2(NodeRect.max.x, NodeRect.position.y + y),
            size :  new Vector2(80, 30),
            title : "Add Choice",
            action : AddChoice,  
            anchor : JAnchor.CenterRight);
            addChoiceButton.Draw();

        float choicesWidth = (ChoiceNodes.Count + 1) * ChoiceNode.DEFAULT_WIDTH;
        SetNodeRectSize(new Vector2(Width < choicesWidth ? choicesWidth : Width, y));
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