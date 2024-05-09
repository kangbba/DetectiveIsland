using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class NodeGroup : Node
{
    public List<Node> nodes;

    public NodeGroup(string _title) : base(_title)
    {


    }

    public override Vector2 CalNodeSize()
    {
        throw new System.NotImplementedException();
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class ChoiceSetNode : Node
{
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();
    public List<NodeGroup> nodeGroups;

    public void AddChoicePlan()
    {
         
    }

    public override Element ToElement()
    {
        ChoiceSet choiceSet = new ChoiceSet(null,null);
        choiceSet.Dialogues = new List<Dialogue>();
        for (int i = 0; i < dialogueNodes.Count; i++)
        {
            choiceSet.Dialogues.Add(dialogueNodes[i].ToElement() as Dialogue);
        }

        choiceSet.Choices = new List<Choice>();
        for (int i = 0; i < nodeGroups.Count; i++)
        {
            Choice choice = new Choice(nodeGroups[i].title,null);
            for (int j = 0; j < nodeGroups[i].nodes.Count; j++)
            {
                choice.Elements.Add(nodeGroups[i].nodes[j].ToElement());
            }
            choiceSet.Choices.Add(choice);
        }
        return choiceSet;
    }



    public ChoiceSetNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(CalNodeSize());
        UpdateNodePosition(pos);
    }

    public override Vector2 CalNodeSize()
    {
        float y = NodeService.CalNodesSizeY(dialogueNodes.Cast<Node>().ToList()); 

        return new Vector2(800, 400 + y);
    }

    public const float dialoguesDist = 10;

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.choiceSetColor;

        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        float accumulatedHeight = 0;
        for (int i = 0; i < dialogueNodes.Count; i++)
        {
            DialogueNode node = dialogueNodes[i];
            node.DrawNode(offset);
            float xPos = NodeCenterX - node.nodeSize.x * 0.5f;

            float yPos = position.y + 100 + accumulatedHeight;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.UpdateNodePosition(dialogue_I_Pos);

            accumulatedHeight += node.CalNodeSize().y + dialoguesDist;
        }
        DrawAddDialogueButton();
        DrawAddNodeSet();
        DrawConnectionPoints(representColor, true, true);

    }

    private void DrawAddNodeSet()
    {
        float y = NodeService.CalNodesSizeY(dialogueNodes.Cast<Node>().ToList());
        Rect buttonRect = new Rect(Rect.center.x - 50, position.y + offset.y + y + 400 + dialoguesDist * dialogueNodes.Count, 100, 20); // Position below the node
        if (GUI.Button(buttonRect, "Add NodeSet"))
        {

        }
    }

    private void DrawAddDialogueButton()
    {
        float y = NodeService.CalNodesSizeY(dialogueNodes.Cast<Node>().ToList());
        Rect buttonRect = new Rect(Rect.center.x - 50, position.y + offset.y  +  y + 200 + dialoguesDist * dialogueNodes.Count, 100, 20); // Position below the node
        if (GUI.Button(buttonRect, "Add Dialogue"))
        {
            DialogueNode dialogueNode = NewDialogueNode(Rect.center);
            dialogueNodes.Add(dialogueNode);
        }
    }

    private DialogueNode NewDialogueNode(Vector2 position)
    {
        DialogueNode node = new DialogueNode(position, "Dialogue");
        node.SetGuid();
        node.dialogue = new Dialogue("Mono", new List<Line>() { });
        return node;
    }

}