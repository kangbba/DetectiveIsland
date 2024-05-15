using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChoiceNode : Node
{
    public string Content = "선택지 내용";
    public List<Node> Nodes = new List<Node>();
    public const float ChoiceNodeWidth = DialogueNode.LINE_NODE_WIDTH + ChoiceSetNode.ChoiceNodeSpacingX * 2;
    public const float NodesSpacingY = 20;

    public float ChoicesTotalSizeY => NodeService.CalNodesSize(Nodes).y;
    
    public ChoiceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        float y = (Nodes != null && Nodes.Count > 0) ? 150 + NodeService.CalNodesSize(Nodes, new(0, NodesSpacingY) ).y : 150;

        return new Vector2(ChoiceNodeWidth, y);
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Content = (string)CustomField("Title : ", Content, Vector2.down * 0f);
        float accumulatedHeight = 0f;
        for(int i = 0 ; i < Nodes.Count ; i++){
            Node node = Nodes[i];
            float xPos = NodeRect.position.x + NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + 100 + accumulatedHeight;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            accumulatedHeight += node.CalNodeSize().y + NodesSpacingY;
        }

        DrawAddDialogueButton();
    }


    private void DrawAddDialogueButton()
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        float y = NodeRect.position.y - 30; 
        Rect buttonRect = new Rect(NodeRect.position.x, y, 60, 20); // Position below the node
        if (GUI.Button(buttonRect, "Dialogue", gUIStyle))
        {
            DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
            Nodes.Add(dialogueNode);
            RefreshNodeSize();
        }
    }



}
