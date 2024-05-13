using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.AssetImporters;
using UnityEditor;

public static class NodeService
{
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
        return node.ToElement();
        /*
        if (node is DialogueNode dialogueNode)
        {
            return dialogueNode.ToElement();
        }
        else if (node is ChoiceSetNode choiceSetNode)
        {
            return choiceSetNode.ToElement();
        }
        else if (node is ItemDemandNode itemDemandNode)
        {
            return itemDemandNode.ToElement();
        }
        else if (node is PositionInitNode positionInitNode)
        {
            return positionInitNode.ToElement();
        }
        else if (node is ItemModifyNode assetChangeNode)
        {
            return assetChangeNode.ToElement();
        }
        else if (node is FriendshipModifyNode assetChangeNode)
        {
            return assetChangeNode.ToElement();
        }
        else if (node is PlaceModifyNode assetChangeNode)
        {
            return assetChangeNode.ToElement();
        }
        else if (node is  assetChangeNode)
        {
            return assetChangeNode.ToElement();
        }
        else
        {
            return null;
        }*/
    }
}



public static class Texture
{
    public static Texture2D CircleTexture { get; private set; }
    private static Dictionary<Color, Texture2D> _flatTextures = new Dictionary<Color, Texture2D>();

    static Texture()
    {
        CircleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/JNode/Icons/JNodeCircle.png");
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
        Texture2D baseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Icons/WhiteTexture.png");
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


public static class NodeColor
{
    public static Color nodeBackgroundColor = new Color(0.09803922f, 0.09803922f, 0.09803922f, 1f);
    public static Color selectedColor = Color.cyan;

    public static Color startNodeColor = new Color(0.4f, 0.8f, 0.4f); // Soft Green
    public static Color dialogueColor = new Color(0.4f, 0.6f, 1.0f); // Soft Blue
    public static Color choiceSetColor = new Color(0.8f, 0.6f, 0.4f); // Soft Orange
    public static Color itemModifyColor = new Color(0.3f, 0.7f, 0.7f); // Teal
    public static Color positionInitColor = new Color(0.9f, 0.9f, 0.4f); // Soft Yellow
    public static Color itemDemandColor = new Color(1.0f, 0.5f, 0.5f); // Soft Red

    public static Color placeModifyColor = new Color(0.5f, 0.7f, 0.9f); // Soft Light Blue
    public static Color friendshipModifyColor = new Color(0.7f, 0.4f, 0.7f); // Soft Mauve
    public static Color overlayPictureColor = new Color(0.5f, 0.1f, 0.8f); // Soft Cyan
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
