using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.AssetImporters;

public static class NodeService
{
    public static Vector2 CalStringVisualSize(GUIStyle style, string st)
    {
        return style.CalcSize(new GUIContent(st));
    }
    public static List<Element> ToElements(this List<Node> nodes)
    {

        List<Element> list = new List<Element>();

        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            list.Add(node.ToProperElement());
        }
        return list;
    }

    public static Element ToProperElement(this Node node)
    {
        if (node is DialogueNode dialogueNode)
        {
            return dialogueNode.dialogue;
        }
        else if (node is ChoiceSetNode choiceSetNode)
        {
            return choiceSetNode.choiceSet;
        }
        else if (node is ItemDemandNode itemDemandNode)
        {
            return itemDemandNode.itemDemand;
        }
        else if (node is PositionChangeNode positionChangeNode)
        {
            return positionChangeNode.positionChange;
        }
        else if (node is AssetChangeNode assetChangeNode)
        {
            return assetChangeNode.assetChange;
        } 
        else{
            return null;
        }
    }
}


