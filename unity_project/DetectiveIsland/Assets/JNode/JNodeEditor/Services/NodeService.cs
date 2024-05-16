using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.AssetImporters;
using UnityEditor;
using System.Linq;

public static class NodeService
{
    public static List<Element> ToElements(this List<Node> nodes)
    {

        List<Element> list = new List<Element>();

        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            list.Add(node.ToElement());
        }
        return list;
    }
    public static Node GetMouseOverNode(List<Node> nodes, Vector2 mousePos)
    {
        int cnt = nodes.Count;
        for(int i = cnt - 1 ; i >= 0 ; i--){
            Node node = nodes[i];
            if(node.IsMouseOver(mousePos)){
                return node;
            }
        }
        return null;
    }
    public static List<Node> GetNodeRectsByPositionX(List<Node> nodes, float x, bool getSmallerNodes)
    {
        List<Node> filteredNodes = new List<Node>();

        foreach (Node node in nodes)
        {
            if (getSmallerNodes)
            {
                if (node.NodeRect.position.y < x)
                {
                    filteredNodes.Add(node);
                }
            }
            else
            {
                if (node.NodeRect.position.y >= x)
                {
                    filteredNodes.Add(node);
                }
            }
        }

        return filteredNodes;
    }
    public static List<Node> GetNodeRectsByPositionY(List<Node> nodes, float y, bool getSmallerNodes)
    {
        List<Node> filteredNodes = new List<Node>();

        foreach (Node node in nodes)
        {
            if (getSmallerNodes)
            {
                if (node.NodeRect.position.y < y)
                {
                    filteredNodes.Add(node);
                }
            }
            else
            {
                if (node.NodeRect.position.y >= y)
                {
                    filteredNodes.Add(node);
                }
            }
        }

        return filteredNodes;
    }
    public static List<Node> MoveNodes(List<Node> nodes, Vector2 delta)
    {
        List<Node> filteredNodes = new List<Node>();

        foreach (Node node in nodes)
        {
            node.SetRectPos(node.NodeRect.position + delta, JAnchor.TopLeft);
        }

        return filteredNodes;
    }
    public static ConnectingPoint GetMouseOverConnectingPoint(List<Node> nodes, Vector2 mousePos)
    {
        int cnt = nodes.Count;
        for(int i = 0 ; i < cnt ; i++){
            Node node = nodes[i];
            if(node.PreviousConnectingPoint.IsContainRect(mousePos)){
                return node.PreviousConnectingPoint;
            }
            if(node.NextConnectingPoint.IsContainRect(mousePos)){
                return node.NextConnectingPoint;
            }
        }
        return null;
    }

    public static float GetNodesWidth(this IEnumerable<Node> nodes){
        float sum = 0f;
        var nodeList = nodes.ToList(); 
        for(int i = 0 ; i < nodeList.Count ; i++){
            sum += nodeList[i].Height;
        }
        return sum;
    }
    public static float GetNodesHeight(this IEnumerable<Node> nodes){
        float sum = 0f;
        var nodeList = nodes.ToList(); 
        for(int i = 0 ; i < nodeList.Count ; i++){
            sum += nodeList[i].Width;
        }
        return sum;
    }
    public static float GetMaxHeight(this IEnumerable<Node> nodes)
    {
        float maxHeight = 0f;
        foreach (var node in nodes)
        {
            if (node.Height > maxHeight)
            {
                maxHeight = node.Height;
            }
        }
        return maxHeight;
    }

    public static float GetMaxWidth(this IEnumerable<Node> nodes)
    {
        float maxWidth = 0f;
        foreach (var node in nodes)
        {
            if (node.Width > maxWidth)
            {
                maxWidth = node.Width;
            }
        }
        return maxWidth;
    }
}



public static class Texture
{
    public static Texture2D CircleTexture { get; private set; }
    private static Dictionary<Color, Texture2D> _flatTextures = new Dictionary<Color, Texture2D>();

    static Texture()
    {
        CircleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/JNode/Textures/JNodeCircle.png");
    }

    public static Texture2D GetBoxTexture(Color color)
    {
        if (!_flatTextures.TryGetValue(color, out Texture2D texture2D))
        {
            texture2D = LoadAndCreateTexture(color);
            if (texture2D != null)
            {
                _flatTextures[color] = texture2D;
            }
        }
        return texture2D;
    }
    private static Texture2D LoadAndCreateTexture(Color color)
    {
        Texture2D baseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/JNode/Textures/WhiteTexture.png");
        if (baseTexture == null)
        {
            Debug.LogError("Failed to load the base white texture.");
            return null;
        }
        Texture2D newTexture = UnityEngine.Object.Instantiate(baseTexture); // Create a copy of the texture to avoid modifying the original asset
        newTexture.SetPixel(0, 0, color);
        newTexture.Apply();
        return newTexture;
    }


}

public static class NodeGuiService
{
    public static Rect AdjustSize(this Rect rect, float width, float height)
    {
        Rect adjustedRect = rect;
        adjustedRect.x -= width * 0.5f;
        adjustedRect.y -= height * 0.5f;
        adjustedRect.width += width;
        adjustedRect.height += height;
        return adjustedRect;
    }
}
