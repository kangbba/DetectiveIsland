using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlaceModifyNode : Node
{
    public bool isGain = true;
    public string placeId;

    public PlaceModifyNode(Vector2 pos, string title) : base(title)
    {
        this.position = pos;
        UpdateNodeSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(300, 150);  // More space for details
    }

    public override Element ToElement()
    {
        return new PlaceModify(isGain, placeId);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.placeModifyColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, CalNodeSize());
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 10, normal = { textColor = Color.white } };
        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 12 };

        float yPos = nodeTotalRect.y + 30 + 20;

        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Gain Type:", labelStyle);
        isGain = EditorGUI.Toggle(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), isGain);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Place ID:", labelStyle);
        placeId = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), placeId, textFieldStyle);

        DrawConnectionPoints(representColor, true, true);
    }
}
