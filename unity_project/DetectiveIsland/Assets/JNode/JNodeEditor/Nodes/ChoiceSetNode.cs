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
    float ChoicesTotalSizeX =>  NodeService.CalNodesSize(ChoiceNodes.Cast<Node>().ToList()).x;
    float ChoicesTotalSizeY => ChoiceNodes.Any() ? ChoiceNodes.Max(node => node.ChoicesTotalSizeY) : 0;
    float DialogueTotalSizeY => NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;   
    
    const float WIDTH = 800;
    
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
        return new Vector2(ChoicesTotalSizeX > 800 ? ChoicesTotalSizeX : 800, 400 + ChoicesTotalSizeY + DialogueTotalSizeY);
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
            float xPos = NodeRect.center.x - NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + 100 + accumulatedHeight;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            accumulatedHeight += node.CalNodeSize().y + dialoguesDist;
        }
        for(int i = 0 ; i < ChoiceNodes.Count ; i++){
            ChoiceNodes[i].DrawNode();
            Vector2 pos = new Vector2( WIDTH * i / (ChoiceNodes.Count - 1) , DialogueTotalSizeY);
            ChoiceNodes[i].SetRectPos(NodeRect.position + pos);
        }
        DrawAddDialogueButton();
        DrawChoiceButton();
    }
    private void DrawAddDialogueButton()
    {
        float y = NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;
        Rect buttonRect = new Rect(NodeRect.center.x - 50, DialogueTotalSizeY + NodeRect.position.y + 150, 100, 20); // Position below the node
        if (GUI.Button(buttonRect, "Add Dialogue"))
        {
            DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
            DialogueNodes.Add(dialogueNode);
        }
        RefreshNodeSize();
    }

    private void DrawChoiceButton()
    {
        float y = NodeService.CalNodesSize(ChoiceNodes.Cast<Node>().ToList()).y;
        Rect buttonRect = new Rect(NodeRect.center.x - 50, DialogueTotalSizeY + NodeRect.position.y +  ChoicesTotalSizeY + 200 , 100, 20); // Position below the node
        if (GUI.Button(buttonRect, "Add Choice"))
        {
            ChoiceNode choiceNode = new ChoiceNode(Guid.NewGuid().ToString(), "ChoiceNode", NodeID);
            ChoiceNodes.Add(choiceNode);
        }
        Debug.Log(ChoicesTotalSizeY);
        RefreshNodeSize();
    }

    



}