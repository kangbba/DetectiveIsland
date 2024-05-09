using UnityEngine;
using UnityEditor;

[System.Serializable]
public class OverlayPictureNode : Node
{
    public string effectID;
    public string pictureID;

    public OverlayPictureNode(Vector2 pos, string title) : base(title)
    {
        this.position = pos;
        UpdateNodeSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(250, 150);  // Space for extra fields
    }

    public override Element ToElement()
    {
        return new OverlayPicture(effectID, pictureID);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.overlayPictureColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, CalNodeSize());
        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 12 };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 10, normal = { textColor = Color.white } };

        float yPos = nodeTotalRect.y + 30 + 20;

        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Effect ID:", labelStyle);
        effectID = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), effectID, textFieldStyle);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Picture ID:", labelStyle);
        pictureID = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), pictureID, textFieldStyle);

        DrawConnectionPoints(representColor, true, true);
    }
}
