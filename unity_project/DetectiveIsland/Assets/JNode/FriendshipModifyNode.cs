using UnityEngine;
using UnityEditor;

[System.Serializable]
public class FriendshipModifyNode : Node
{
    public bool isGain = true;
    public string characterID;
    public int amount = 10;

    public FriendshipModifyNode(Vector2 pos, string title) : base(title)
    {
        this.position = pos;
        UpdateNodeSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(250, 150);  // Adjusted size for additional input fields
    }

    public override Element ToElement()
    {
        return new FriendshipModify(isGain, characterID, amount);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.friendshipModifyColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, CalNodeSize());
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 10, normal = { textColor = Color.white } };
        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField) { fontSize = 12 };

        float yPos = nodeTotalRect.y + 30 + 20;

        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Gain Type:", labelStyle);
        isGain = EditorGUI.Toggle(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), isGain);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Character ID:", labelStyle);
        characterID = EditorGUI.TextField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), characterID, textFieldStyle);

        yPos += 25;
        EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, 80, 20), "Amount:", labelStyle);
        amount = EditorGUI.IntField(new Rect(nodeTotalRect.x + 85, yPos, 150, 20), amount);

        DrawConnectionPoints(representColor, true, true);
    }
}
