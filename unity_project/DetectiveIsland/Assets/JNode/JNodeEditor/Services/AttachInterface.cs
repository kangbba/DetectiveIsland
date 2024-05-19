using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class AttachInterface 
{ 
    #region  버튼 그룹 

      public static void AttachBtnGroups(Vector2 nodeRectPos, Vector2 btnSize, List<Node> nodes, string parentNodeID)
    {
        var buttonData = new (string title, Action<List<Node>, string, Vector2> action)[]
        {
            ("Dialogue", AddDialogueNode),
            ("Item Modi", AddGainItemNode),
            ("Pos Init", AddModifyPositionNode),
            ("FriendShip", AddGainFriendshipNode),
            ("PlaceModi", AddGainPlaceNode),
            ("Overlay Pic", AddOverlayPictureNode),
            ("Overlay Sen", AddOverlayPictureNode),
            ("Audio Act", AddAudioActionNode),
            ("Camera Act", AddCameraActionNode)
            ,
        };

        for (int i = 0; i < buttonData.Length; i++)
        {
            new JButton(
                pos: new Vector2(nodeRectPos.x + 5 * i + btnSize.x * i, nodeRectPos.y - 30),
                size: btnSize,
                title: buttonData[i].title,
                action: () => buttonData[i].action(nodes, parentNodeID, new Vector2(nodeRectPos.x + 10 * i + btnSize.x * i, nodeRectPos.y)),
                anchor: JAnchor.TopLeft
            ).Draw();
        }
    }
    public static void ProcessContextMenu(List<Node> Nodes, Vector2 mousePos)
    {
       GenericMenu menu = new GenericMenu();

        // Add nodes directly
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, () => { AddDialogueNode(Nodes, null, mousePos);});
        menu.AddItem(new GUIContent("Add ChoiceSet Node"), false, () => AddChoiceSetNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("Add ItemDemand Node"), false, () => AddItemDemandNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("Add CameraAction Node"), false, () => AddCameraActionNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("Add AudioAction Node"), false, () => AddAudioActionNode(Nodes, null, mousePos));

        // Add Gain nodes as sub-menu
        menu.AddItem(new GUIContent("획득하기/Add GainItem Node"), false, () => AddGainItemNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("획득하기/Add GainPlace Node"), false, () => AddGainPlaceNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("획득하기/Add GainFriendship Node"), false, () => AddGainFriendshipNode(Nodes, null, mousePos));

        menu.AddItem(new GUIContent("수정하기/Add ModifyPosition Node"), false, () => AddModifyPositionNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("수정하기/Add OverlayPicture Node"), false, () => AddOverlayPictureNode(Nodes, null, mousePos));
        menu.AddItem(new GUIContent("수정하기/Add OverlaySentence Node"), false, () => AddOverlaySentenceNode(Nodes, null, mousePos));

        menu.ShowAsContext();

    }
    public static Action<List<Node>, string, Vector2> AddChoiceSetNode = (nodes, parentNodeID, pos) =>
    {
        ChoiceSetNode node = new ChoiceSetNode(Guid.NewGuid().ToString(), "ChoiceSetNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddItemDemandNode = (nodes, parentNodeID, pos) =>
    {
        ItemDemandNode node = new ItemDemandNode(Guid.NewGuid().ToString(), "ItemDemandNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddDialogueNode = (nodes, parentNodeID, pos) =>
    {
        DialogueNode node = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddGainItemNode = (nodes, parentNodeID, pos) =>
    {
        GainItemNode node = new GainItemNode(Guid.NewGuid().ToString(), "GainItemNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddModifyPositionNode = (nodes, parentNodeID, pos) =>
    {
        ModifyPositionNode node = new ModifyPositionNode(Guid.NewGuid().ToString(), "ModifyPositionNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddGainFriendshipNode = (nodes, parentNodeID, pos) =>
    {
        GainFriendshipNode node = new GainFriendshipNode(Guid.NewGuid().ToString(), "GainFriendshipNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddGainPlaceNode = (nodes, parentNodeID, pos) =>
    {
        GainPlaceNode node = new GainPlaceNode(Guid.NewGuid().ToString(), "GainPlaceNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddOverlayPictureNode = (nodes, parentNodeID, pos) =>
    {
        OverlayPictureNode node = new OverlayPictureNode(Guid.NewGuid().ToString(), "OverlayPictureNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddOverlaySentenceNode = (nodes, parentNodeID, pos) =>
    {
        OverlaySentenceNode node = new OverlaySentenceNode(Guid.NewGuid().ToString(), "OverlaySentenceNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddAudioActionNode = (nodes, parentNodeID, pos) =>
    {
        AudioActionNode node = new AudioActionNode(Guid.NewGuid().ToString(), "AudioActionNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    public static Action<List<Node>, string, Vector2> AddCameraActionNode = (nodes, parentNodeID, pos) =>
    {
        CameraActionNode node = new CameraActionNode(Guid.NewGuid().ToString(), "CameraActionNode", parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    };

    #endregion

    #region  지우기 버튼, 순서 바꾸기 버튼
    public static void AttachDeleteButtons<T>(List<T> nodes, Vector2 btnSize = default) where T : Node
    {
        if(btnSize == default){
            btnSize = Vector2.one * 20f;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            T node = nodes[i];
            if (node is T lineNode)
            {
                string nodeId = lineNode.NodeID;
                JButton deleteBtn = new JButton(
                    pos: new Vector2(lineNode.NodeRect.max.x, lineNode.NodeRect.position.y),
                    size: btnSize,
                    title: "X",
                    anchor: JAnchor.TopRight,
                    action: () => DeleteLineNode(nodes, nodeId)
                );
                deleteBtn.Draw();
            }
        }
    }

    public static void DeleteLineNode<T>(List<T> nodes, string nodeId) where T : Node
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }

        T nodeToDelete = nodes.FirstOrDefault(node => node.NodeID == nodeId);
        if (nodeToDelete != null)
        {
            nodes.Remove(nodeToDelete);
        }
    }
    
    public static void AttachArrowButtons<T>(List<T> nodes, Vector2 btnSize = default) where T : Node
    {
        if(btnSize == default){
            btnSize = Vector2.one * 20f;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            T node = nodes[i];
            if (node is T lineNode)
            {
                string nodeId = lineNode.NodeID;
                JButton orderUpBtn = new JButton(
                  pos: new Vector2(lineNode.NodeRect.max.x - btnSize.x * 1, lineNode.NodeRect.position.y),
                  size: btnSize,
                  title: "▲",
                  anchor: JAnchor.TopRight,
                  action: () => MoveListOrder(nodes, lineNode.NodeID, -1)
                ); 
                orderUpBtn.Draw();

                JButton orderDownBtn = new JButton(
                  pos: new Vector2(lineNode.NodeRect.max.x - btnSize.x * 2, lineNode.NodeRect.position.y),
                  size: btnSize,
                  title: "▼",
                  anchor: JAnchor.TopRight,
                  action: () => MoveListOrder(nodes, lineNode.NodeID, 1)
                );
                orderDownBtn.Draw();
            }
        }
    }

    public static void MoveListOrder<T>(List<T> nodes, string nodeId, int direction) where T : Node
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }

        int index = nodes.FindIndex(node => node.NodeID == nodeId);
        if (index == -1)
        {
            Debug.LogWarning("Node not found");
            return;
        }

        int newIndex = index + direction;

        // Ensure newIndex is within valid range
        if (newIndex < 0 || newIndex >= nodes.Count)
        {
            Debug.LogWarning("Invalid move direction");
            return;
        }

        // Release focus from the current text area
        GUI.FocusControl(null);

        // Swap the elements to change the order
        T nodeToMove = nodes[index];
        nodeToMove.Notice();
        nodes.RemoveAt(index);
        nodes.Insert(newIndex, nodeToMove);
    }
    #endregion
}
