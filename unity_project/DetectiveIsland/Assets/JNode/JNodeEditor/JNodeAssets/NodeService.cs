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
            list.Add(node.ToElement());
        }
        return list;
    }
    public static Vector2 CalNodesSize(List<Node> nodes)
    {
        Vector2 result = Vector2.zero;
        foreach (Node node in nodes)
        {
            result += node.CalNodeSize();
        }
        return result;
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
