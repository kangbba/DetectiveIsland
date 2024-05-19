using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class AttachInterface 
{ 
    #region  버튼 그룹 

    private static readonly Dictionary<string, Action<List<Node>, string, Vector2>> nodeActions = new Dictionary<string, Action<List<Node>, string, Vector2>>
    {
        { "Dialogue",            (nodes, parentNodeID, pos)         =>  CreateAndAddNode<DialogueNode>(nodes, parentNodeID, pos)         },
        { "ChoiceSet",           (nodes, parentNodeID, pos)         =>  CreateAndAddNode<ChoiceSetNode>(nodes, parentNodeID, pos)        },
        { "ItemDemand",          (nodes, parentNodeID, pos)         =>  CreateAndAddNode<ItemDemandNode>(nodes, parentNodeID, pos)       },
        { "CameraAction",        (nodes, parentNodeID, pos)         =>  CreateAndAddNode<CameraActionNode>(nodes, parentNodeID, pos)     },
        { "AudioAction",         (nodes, parentNodeID, pos)         =>  CreateAndAddNode<AudioActionNode>(nodes, parentNodeID, pos)      },
        { "GainItem",            (nodes, parentNodeID, pos)         =>  CreateAndAddNode<GainItemNode>(nodes, parentNodeID, pos)         },
        { "GainPlace",           (nodes, parentNodeID, pos)         =>  CreateAndAddNode<GainPlaceNode>(nodes, parentNodeID, pos)        },
        { "GainFriendship",      (nodes, parentNodeID, pos)         =>  CreateAndAddNode<GainFriendshipNode>(nodes, parentNodeID, pos)   },
        { "ModifyPosition",      (nodes, parentNodeID, pos)         =>  CreateAndAddNode<ModifyPositionNode>(nodes, parentNodeID, pos)   },
        { "OverlayPicture",      (nodes, parentNodeID, pos)         =>  CreateAndAddNode<OverlayPictureNode>(nodes, parentNodeID, pos)   },
        { "OverlaySentence",     (nodes, parentNodeID, pos)         =>  CreateAndAddNode<OverlaySentenceNode>(nodes, parentNodeID, pos)  }
    };

    public static void ShowContextMenu(List<Node> nodes, Node parentNode, Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();

        // Check if parentNode is null and set the appropriate text
        bool isMostParentNode = parentNode == null;
        string parentNodeText = isMostParentNode ? "최상위에 생성합니다" : $"{parentNode.Title} 안에 생성합니다";

        // Add the parentNodeText as a disabled item to the top of the menu
        menu.AddDisabledItem(new GUIContent(parentNodeText));

        // Define a list of node types to exclude when not at the most parent level
        var excludedNodes = new HashSet<string> { "ChoiceSet", "ItemDemand" };

        foreach (var kvp in nodeActions)
        {
            // Check if the current node type should be excluded
            if (!isMostParentNode && excludedNodes.Contains(kvp.Key))
            {
                continue;
            }

            string title = $"Add {kvp.Key} Node";
            string nodeID = isMostParentNode ? "" : parentNode.NodeID;
            Action<List<Node>, string, Vector2> action = kvp.Value;
            menu.AddItem(new GUIContent(title), false, () => action(nodes, nodeID, mousePos));
        }

        menu.ShowAsContext();
    }


    private static void CreateAndAddNode<T>(List<Node> nodes, string parentNodeID, Vector2 pos) where T : Node
    {
        string nodeName = typeof(T).Name;
        T node = (T)Activator.CreateInstance(typeof(T), Guid.NewGuid().ToString(), nodeName, parentNodeID);
        node.SetRectPos(pos, JAnchor.TopLeft);
        node.Notice();
        nodes.Add(node);
    }


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
