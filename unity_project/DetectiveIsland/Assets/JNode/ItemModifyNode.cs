using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ItemModifyNode : Node
{
    public bool isGain = true;
    public string itemID;
    public int itemAmount;

    public ItemModifyNode(Vector2 pos, string title) : base(title)
    {
        this.position = pos;
        UpdateNodeSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return new ItemModify(isGain, itemID, itemAmount);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(250, 150);  // Adjusted for additional input fields
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.itemModifyColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, CalNodeSize());

        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 12
        };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 10,
            normal = { textColor = Color.white }
        };

        float yPos = nodeTotalRect.y + 50;

        // Gain Type Toggle
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Gain Type:", labelStyle);
        isGain = EditorGUI.Toggle(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), isGain);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Item ID:", labelStyle);
        itemID = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), itemID, textFieldStyle);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Item Amount:", labelStyle);
        itemAmount = EditorGUI.IntField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), itemAmount);

        DrawConnectionPoints(representColor, true, true);
    }
}