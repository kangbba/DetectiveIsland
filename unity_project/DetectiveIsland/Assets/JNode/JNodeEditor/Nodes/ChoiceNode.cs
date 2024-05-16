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

    public override float Width { get; set; }
    public override float Height { get; set; }

    public const float DEFAULT_WIDTH = 500;
    public const float CONTENT_LABLE_WIDTH = 60;
    public const float CONTENT_FIELD_WIDTH = 300;

    public const float CONTENT_FIELD_HEIGHT = 80;
    public const float CONTENT_UPPER_MARGIN = 30;
    public const float CONTENT_BOTTOM_MARGIN = 30;

    public const float ADD_BTN_WIDTH = 60;
    public const float ADD_BTN_HEIGHT = 20;


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
        Width = DEFAULT_WIDTH;
        Height = UPPER_MARGIN;
        Height += CONTENT_UPPER_MARGIN;
        Content = (string)JInterface.SimpleField
        (
            value : Content,
            pos : new Vector2(NodeRect.position.x + 100, NodeRect.position.y + Height),
            title : "Content : ",
            labelWidth : CONTENT_LABLE_WIDTH,
            fieldWidth : CONTENT_FIELD_WIDTH,
            fieldHeight : CONTENT_FIELD_HEIGHT
        );
        Height += CONTENT_FIELD_HEIGHT;
        Height += CONTENT_BOTTOM_MARGIN;
        for (int i = 0 ; i < Nodes.Count ; i++){
            Node node = Nodes[i];
            float xPos = NodeRect.position.x + NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            Height += node.Height + 10; 
        }
        JInterface.AttachDeleteButtons(Nodes);
        JInterface.AttachArrowButtons(Nodes);
   // Add buttons using JButton
        BtnGroups();

        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }

    private void BtnGroups(){
        new JButton(
            pos: new Vector2(NodeRect.position.x + 00 + ADD_BTN_WIDTH * 0, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "Dialogue",
            action: AddDialogue,
            anchor: JAnchor.TopLeft
        ).Draw();

        new JButton(
            pos: new Vector2(NodeRect.position.x + 10 + ADD_BTN_WIDTH * 1, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "Item Modi",
            action: AddItemModify,
            anchor: JAnchor.TopLeft
        ).Draw();

        new JButton(
            pos: new Vector2(NodeRect.position.x + 20 + ADD_BTN_WIDTH * 2, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "Pos Init",
            action: AddPositionInit,
            anchor: JAnchor.TopLeft
        ).Draw();

        new JButton(
            pos: new Vector2(NodeRect.position.x + 30 + ADD_BTN_WIDTH * 3, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "FriendShip",
            action: AddFriendShipModify,
            anchor: JAnchor.TopLeft
        ).Draw();

        new JButton(
            pos: new Vector2(NodeRect.position.x + 40 + ADD_BTN_WIDTH * 4, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "PlaceModi",
            action: AddPlaceModify,
            anchor: JAnchor.TopLeft
        ).Draw();

        new JButton(
            pos: new Vector2(NodeRect.position.x + 50 + ADD_BTN_WIDTH * 5, NodeRect.position.y - 30),
            size: new Vector2(ADD_BTN_WIDTH, ADD_BTN_HEIGHT),
            title: "Overlay Pic",
            action: AddOverlayPicture,
            anchor: JAnchor.TopLeft
        ).Draw();
    }

    private void AddDialogue()
    {
        DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
        Nodes.Add(dialogueNode);
    }

    private void AddItemModify()
    {
        ItemModifyNode node = new ItemModifyNode(Guid.NewGuid().ToString(), "ItemModifyNode", NodeID);
        Nodes.Add(node);
    }

    private void AddPositionInit()
    {
        PositionInitNode node = new PositionInitNode(Guid.NewGuid().ToString(), "PositionInitNode", NodeID);
        Nodes.Add(node);
    }

    private void AddFriendShipModify()
    {
        FriendshipModifyNode node = new FriendshipModifyNode(Guid.NewGuid().ToString(), "FriendshipModifyNode", NodeID);
        Nodes.Add(node);
    }

    private void AddPlaceModify()
    {
        PlaceModifyNode node = new PlaceModifyNode(Guid.NewGuid().ToString(), "PlaceModifyNode", NodeID);
        Nodes.Add(node);
    }

    private void AddOverlayPicture()
    {
        OverlayPictureNode node = new OverlayPictureNode(Guid.NewGuid().ToString(), "OverlayPictureNode", NodeID);
        Nodes.Add(node);
    }
}
