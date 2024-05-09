using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AssetChangeNode : Node
{
    public string gainType;
    public string itemID;
    public uint itemAmount;

    public AssetChangeNode(Vector2 pos, string title) : base(title)
    {
        this.position = pos;
        UpdateNodeSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return new AssetChange(gainType, itemID, itemAmount);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(250, 200);  // Adjusted for additional input fields
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.assetChangeColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, CalNodeSize());

        GUIStyle textFieldStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white, background = Texture2D.grayTexture },
            fontSize = 12
        };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        float yPos = nodeTotalRect.y + 30 + 40;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Gain Type:", labelStyle);
        gainType = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), gainType, textFieldStyle);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Item ID:", labelStyle);
        itemID = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), itemID, textFieldStyle);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Item Amount:", labelStyle);
        itemAmount = (uint)EditorGUI.IntField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), (int)itemAmount);

        DrawConnectionPoints(representColor, true, true);
    }
}