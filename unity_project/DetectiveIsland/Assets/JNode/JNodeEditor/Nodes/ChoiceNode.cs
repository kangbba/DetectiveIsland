using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChoiceNode : Node
{

    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get => DEFAULT_WIDTH; }
    public override float Height { get => UPPER_MARGIN + NodesHeightSum + BOTTOM_MARGIN;  }

    public const float DEFAULT_WIDTH = 300;
    
    public float NodesHeightSum 
    {
        get
        {
            float sum = 0f;
            for(int i = 0 ; i < Nodes.Count ; i++){
                sum += Nodes[i].Height;
            }
            return sum;
        }
    }

    public string Content = "선택지 내용";
    public List<Node> Nodes = new List<Node>();
    
    public ChoiceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        
        Content = (string)CustomField("Title : ", Content, Vector2.down * 0f);
        float y = UPPER_MARGIN;

        for(int i = 0 ; i < Nodes.Count ; i++){
            Node node = Nodes[i];
            float xPos = NodeRect.position.x + NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + y;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();

            y += node.Height; 
        }

        DrawAddDialogueButton(new Vector2(0, y));
        y += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, y));

    }


    private void DrawAddDialogueButton(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, 60, 20); // Position below the node
        if (GUI.Button(buttonRect, "Dialogue", gUIStyle))
        {
            DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
            Nodes.Add(dialogueNode);
        }
    }



}
