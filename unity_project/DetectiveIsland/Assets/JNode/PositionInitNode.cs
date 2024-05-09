using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




[System.Serializable]
public class PositionInitNode : Node
{
    public List<CharacterPosition> characterPositions = new List<CharacterPosition>();
    public override Element ToElement()
    {
        PositionInit positionInit = new PositionInit(characterPositions);

        return positionInit;
    }

    public void AddCharacterPosition()
    {

    }

    public PositionInitNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(CalNodeSize());
        UpdateNodePosition(pos);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(400, 140 + characterPositions.Count * 20);
    }
    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.positionInitColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        Rect nodeTotalRect = new Rect(position + offset, new Vector2(400, 200 + characterPositions.Count * 25));

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

        // Calculate available width for text fields
        float fieldWidth = (nodeTotalRect.width - 60) / 2; // Reduced width to fit the delete button

        // Starting Y position for character positions
        float yPos = nodeTotalRect.y + 50; // Start 50 pixels down from the top of the node

        for (int i = 0; i < characterPositions.Count; i++)
        {
            // Draw fields for Character ID
            GUIContent labelContentCharacter = new GUIContent("Character ID:");
            Vector2 labelSizeCharacter = labelStyle.CalcSize(labelContentCharacter);
            EditorGUI.LabelField(new Rect(nodeTotalRect.x, yPos, labelSizeCharacter.x, 20), labelContentCharacter, labelStyle);

            characterPositions[i].CharacterID = EditorGUI.TextField(
                new Rect(nodeTotalRect.x + labelSizeCharacter.x + 5, yPos, fieldWidth - labelSizeCharacter.x - 5, 20),
                characterPositions[i].CharacterID,
                textFieldStyle);

            // Draw fields for Position ID
            GUIContent labelContentPosition = new GUIContent("Position ID:");
            Vector2 labelSizePosition = labelStyle.CalcSize(labelContentPosition);
            EditorGUI.LabelField(new Rect(nodeTotalRect.x + fieldWidth + 5, yPos, labelSizePosition.x, 20), labelContentPosition, labelStyle);

            characterPositions[i].PositionID = EditorGUI.TextField(
                new Rect(nodeTotalRect.x + fieldWidth + labelSizePosition.x + 10, yPos, fieldWidth - labelSizePosition.x - 5, 20),
                characterPositions[i].PositionID,
                textFieldStyle);

            // Add a delete button
            if (GUI.Button(new Rect(nodeTotalRect.x + 2 * fieldWidth + 15, yPos, 45, 20), "X"))
            {
                characterPositions.RemoveAt(i);
            }

            yPos += 25; // Increment y position for the next set of inputs
        }

        if (GUI.Button(new Rect(nodeTotalRect.x, yPos, nodeTotalRect.width, 20), "Add Character Position"))
        {
            characterPositions.Add(new CharacterPosition("", "Middle"));
        }

        GUI.color = Color.white;
        DrawConnectionPoints(representColor, true, true);
    }
}

    