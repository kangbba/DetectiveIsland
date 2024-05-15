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
    Vector2 AddDialogueBtnPos => new Vector2(NodeRect.max.x, (DialogueNodes != null && DialogueNodes.Count > 0) ? DialogueNodes.Last().NodeRect.position.y : 100);
    Vector2 AddChoiceSetBtnPos => new Vector2(NodeRect.max.x, (ChoiceNodes != null && ChoiceNodes.Count > 0) ? ChoiceNodes.Last().NodeRect.position.y : 100);
    
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
        float width = NodeService.CalNodesSize(ChoiceNodes.Cast<Node>().ToList()).x + 500;
        float dialogueTotalSizeY = NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;
        float longestChoiceHeight =  ChoiceNodes.Any() ? ChoiceNodes.Max(node => node.ChoicesTotalSizeY) : 0;
        float height = dialogueTotalSizeY + longestChoiceHeight;
        return new Vector2(width, height);
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
            Node node = ChoiceNodes[i];
            node.DrawNode();
            Vector2 pos = new Vector2(NodeRect.min.x + 200 * i, AddChoiceSetBtnPos.y);
            node.SetRectPos(NodeRect.position + pos);
        }
        DrawAddDialogueButton();
        DrawChoiceButton();
    }
    private void DrawAddDialogueButton()
    {
        float y = NodeService.CalNodesSize(DialogueNodes.Cast<Node>().ToList()).y;
        Rect buttonRect = new Rect(AddDialogueBtnPos.x, AddDialogueBtnPos.y, 60, 60); // Position below the node
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
        Rect buttonRect = new Rect(AddChoiceSetBtnPos.x, AddChoiceSetBtnPos.y, 60, 60); // Position below the node
        if (GUI.Button(buttonRect, "Add Choice"))
        {
            ChoiceNode choiceNode = new ChoiceNode(Guid.NewGuid().ToString(), "ChoiceNode", NodeID);
            ChoiceNodes.Add(choiceNode);
        }
        RefreshNodeSize();
    }

    



}