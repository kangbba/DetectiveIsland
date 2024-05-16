using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDemandResponseNode : Node
{

    public const float UPPER_MARGIN = 0;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }

    public const float DEFAULT_WIDTH = 500;

    public const float AddBtnWidth = 60;
    public const float AddBtnHeight = 20;


    public List<Node> Nodes = new List<Node>();

    public ItemDemandResponseNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = DEFAULT_WIDTH;
        Height = UPPER_MARGIN;
        Height += 80;

        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            float xPos = NodeRect.position.x + NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            Height += node.Height + 10;
        }
        DrawAddDialogueButton(new Vector2(NodeRect.position.x + 00 + AddBtnWidth * 0, NodeRect.position.y - 30));
        DrawAddItemModifyBtn(new Vector2(NodeRect.position.x + 10 + AddBtnWidth * 1, NodeRect.position.y - 30));
        DrawAddPositionInitBtn(new Vector2(NodeRect.position.x + 20 + AddBtnWidth * 2, NodeRect.position.y - 30));
        DrawAddFriendShipModifyBtn(new Vector2(NodeRect.position.x + 30 + AddBtnWidth * 3, NodeRect.position.y - 30));
        DrawAddPlaceModifyBtn(new Vector2(NodeRect.position.x + 40 + AddBtnWidth * 4, NodeRect.position.y - 30));
        DrawAddOverlayPictureBtn(new Vector2(NodeRect.position.x + 50 + AddBtnWidth * 5, NodeRect.position.y - 30));

        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }


    private void DrawAddDialogueButton(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "Dialogue", gUIStyle))
        {
            DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
            Nodes.Add(dialogueNode);
        }
    }


    private void DrawAddItemModifyBtn(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "Item Modi", gUIStyle))
        {
            ItemModifyNode node = new ItemModifyNode(Guid.NewGuid().ToString(), "ItemModifyNode", NodeID);
            Nodes.Add(node);
        }
    }
    private void DrawAddPositionInitBtn(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "Pos Init", gUIStyle))
        {
            PositionInitNode node = new PositionInitNode(Guid.NewGuid().ToString(), "PositionInitNode", NodeID);
            Nodes.Add(node);
        }
    }
    private void DrawAddFriendShipModifyBtn(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "FriendShip", gUIStyle))
        {
            FriendshipModifyNode node = new FriendshipModifyNode(Guid.NewGuid().ToString(), "FriendshipModifyNode", NodeID);
            Nodes.Add(node);
        }
    }

    private void DrawAddPlaceModifyBtn(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "PlaceModi", gUIStyle))
        {
            PlaceModifyNode node = new PlaceModifyNode(Guid.NewGuid().ToString(), "PlaceModifyNode", NodeID);
            Nodes.Add(node);
        }
    }

    private void DrawAddOverlayPictureBtn(Vector2 pos)
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
        gUIStyle.normal.background = Texture2D.grayTexture;
        gUIStyle.alignment = TextAnchor.MiddleCenter;
        gUIStyle.fontSize = 9;
        gUIStyle.normal.textColor = Color.white; // 원하는 폰트 색상을 설정합니다.

        Rect buttonRect = new Rect(pos.x, pos.y, AddBtnWidth, AddBtnHeight); // Position below the node
        if (GUI.Button(buttonRect, "Overlay Pic", gUIStyle))
        {
            OverlayPictureNode node = new OverlayPictureNode(Guid.NewGuid().ToString(), "OverlayPictureNode", NodeID);
            Nodes.Add(node);
        }
    }
}
