using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Aroka.ArokaUtils;
using static TreeEditor.TreeEditorHelper;
using UnityEngine.UIElements;
using System.Drawing;
using Color = UnityEngine.Color;
using System;

[System.Serializable]
public class ConnectingPoint
{
    public Rect rect;
    public string connectedNodeId;
    public bool isConnected;

    public string ConnectedNodeId
    {
        get
        {
            return connectedNodeId;
        }
    }

    public ConnectingPoint()
    {
        connectedNodeId = "";
    }

    public void Connect(string connectedNodeId)
    {
        this.connectedNodeId = connectedNodeId;
        isConnected = true;
    }

    public void DeConnect()
    {
        this.connectedNodeId = "";
        isConnected = false;
    }


    public void DrawSingleConnectionPoint(Vector2 centerPoint, Color color)
    {

        float innerRadius = 10;  // 내부 원의 반지름
        float edgeThickness = 1;  // 테두리 두께
        float edgeRadius = innerRadius + edgeThickness;  // 테두리 원의 반지름

        Rect edgeRect = new Rect(centerPoint.x - edgeRadius, centerPoint.y - edgeRadius, edgeRadius * 2, edgeRadius * 2);
        Rect innerCircleRect = new Rect(centerPoint.x - innerRadius, centerPoint.y - innerRadius, innerRadius * 2, innerRadius * 2);
        Texture2D circleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/JNode/Icons/JNodeCircle.png");
        rect = innerCircleRect;
        GUI.color = color;
        GUI.DrawTexture(edgeRect, circleTexture);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.DrawTexture(innerCircleRect, circleTexture);

        GUI.color = Color.white;
    }
}


[System.Serializable]
public class Node
{
    public Rect rect;
    public string title;
    public bool isSelected;
    
    private string id;
    public string ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    private ConnectingPoint _parentConnectingPoint;
    public ConnectingPoint ParentConnectingPoint
    {
        get
        {
            return _parentConnectingPoint ?? (_parentConnectingPoint = new ConnectingPoint());
        }
        set
        {
            _parentConnectingPoint = value;
        }
    }

    private ConnectingPoint _childConnectingPoint;
    public ConnectingPoint ChildConnectingPoint
    {
        get
        {
            return _childConnectingPoint ?? (_childConnectingPoint = new ConnectingPoint());
        }
        set
        {
            _childConnectingPoint = value;
        }
    }


    public Node(Rect rect, string title)
    { 
        this.rect = rect;
        this.title = title;
    }

    public void SetGuid()
    {
        id = Guid.NewGuid().ToString();
        Debug.Log("Set Guid Node" + " | " + title + " | " + id);
    }



    public void ConnectNodeToChild(Node node)
    {
        ChildConnectingPoint.Connect(node.id);
    }
    public void ConnectNodeToParent(Node node)
    {
       
        ParentConnectingPoint.Connect(node.id);
    }

    public void DeConnectNodeChild()
    {
      
        ChildConnectingPoint.DeConnect();
    }

    public void DeConnectNodeParent()
    {
        ParentConnectingPoint.DeConnect();
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    { 
        isSelected = false;
    }

    public virtual void DrawNode(Vector2 offset)
    {

    }
}

public static class NodeColor
{
    public static Color nodeBackgroundColor = new Color(0.09803922f, 0.09803922f, 0.09803922f,1f);
    public static Color selectedColor = Color.cyan;
    public static Color dialogueColor = Color.red;
    public static Color startNodeColor = Color.yellow;

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

[System.Serializable]
public class DialogueNode : Node
{
    public Dialogue dialogue;
    public string characterName;


    public DialogueNode(Rect rect, string title) : base(rect, title)
    {

    }

    public void DrawBackground(Rect nodeTotalRect)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeTotalRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = NodeColor.dialogueColor;
        GUI.Box(nodeTotalRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeTotalRect, "", boxGS);
    }

    public void DrawTitle(Rect nodeTotalRect)
    {

        //타이틀
        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeTotalRect.ModifiedY(nodeTotalRect.y + 30), title, titleGS);
    }

    public void DrawCharacterType(Rect nodeTotalRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        GUIContent labelContent = new GUIContent("Character Name:");
        Vector2 labelSize_CharacterName = labelStyle.CalcSize(labelContent);
        GUI.Label(new Rect(nodeTotalRect.x, nodeTotalRect.y + 60, labelSize_CharacterName.x, 20), labelContent, labelStyle);
        // Character Name Input Field
        characterName = GUI.TextField(new Rect(nodeTotalRect.x + labelSize_CharacterName.x + 5, nodeTotalRect.y + 60, nodeTotalRect.width - labelSize_CharacterName.x - 10, 20), characterName);
    }

    public void DrawAddLineButton(Rect nodeTotalRect)
    {
        // Define the label style and text right within the function
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        string labelText = "Character Name:";
        GUIContent labelContent = new GUIContent(labelText);
        Vector2 labelSize = labelStyle.CalcSize(labelContent);

        // Calculate Y position based on where your label is actually positioned, adjust if needed
        float labelYPosition = nodeTotalRect.y +35 + (dialogue.Lines.Count + 1) * 55 ;  // Example Y position
        float buttonWidth = 200;
        float buttonHeight = 50;

        // Position the button centered relative to the node width, under the label
        Rect plusButtonRect = new Rect(
            nodeTotalRect.x + (nodeTotalRect.width - buttonWidth) / 2,  // Center horizontally
            labelYPosition,  // Position below the label with a small gap
            buttonWidth,
            buttonHeight);

        if (GUI.Button(plusButtonRect, "+"))
        {
            OnPlusButtonClicked(); // Function to handle the button click
        }
    }/*
    public void DrawLine(Rect nodeTotalRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        Debug.Log(dialogue.Lines.Count);
        GUI.Box(new Rect(nodeTotalRect.x, nodeTotalRect.y + 35 + (dialogue.Lines.Count * 55), 200, 50), "line");
        // Character Name Input Field
    }*/
    public void DrawLine(Rect nodeTotalRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };


        // Loop through all lines to draw each one
        for (int i = 0; i < dialogue.Lines.Count; i++)
        {
            float offsetY = 35 + (i * 55); // Calculate Y offset for each line
            Rect lineRect = new Rect(nodeTotalRect.x, nodeTotalRect.y + 35 + (dialogue.Lines.Count * 55), 200, 50);
            GUI.Box(lineRect, "Line"); // Draw box for the line

            // Emotion ID Input Field
            GUIContent emotionLabelContent = new GUIContent("Emotion ID:");
            GUI.Label(new Rect(nodeTotalRect.x + 5, nodeTotalRect.y + offsetY + 80, 80, 20), emotionLabelContent, labelStyle);
            dialogue.Lines[i].EmotionID = (GUI.TextField(new Rect(nodeTotalRect.x + 85, nodeTotalRect.y + offsetY + 5, 110, 20), dialogue.Lines[i].EmotionID, 25));

            // Sentence Input Field
            GUIContent sentenceLabelContent = new GUIContent("Sentence:");
            GUI.Label(new Rect(nodeTotalRect.x + 5, nodeTotalRect.y + offsetY + 100, 80, 20), sentenceLabelContent, labelStyle);
            dialogue.Lines[i].Sentence = (GUI.TextField(new Rect(nodeTotalRect.x + 85, nodeTotalRect.y + offsetY + 5, 110, 20), dialogue.Lines[i].Sentence, 25));
        }
    }


    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.dialogueColor;

        base.DrawNode(offset );

        Rect nodeTotalRect = new Rect((rect.position + offset ) , rect.size );
        DrawBackground(nodeTotalRect);
        DrawTitle(nodeTotalRect);
        DrawCharacterType(nodeTotalRect);
        DrawAddLineButton(nodeTotalRect);
        DrawLine(nodeTotalRect);

        GUI.color = Color.white;
        base.ParentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
   

    private void OnPlusButtonClicked()
    {
        Debug.Log("Plus button clicked in Dialogue Node");
    }
}

[System.Serializable]
public class StartNode : Node
{
    public StartNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect nodeRect = new Rect((rect.position + offset), rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        //base.parentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
}



[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;
    public ChoiceSetNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {

        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect nodeRect = new Rect((rect.position + offset), rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        //base.parentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
}

[System.Serializable]
public class ItemDemandNode : Node
{
    public ItemDemand itemDemand;
    public ItemDemandNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect nodeRect = new Rect((rect.position + offset), rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        //base.parentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
}


[System.Serializable]
public class PositionChangeNode : Node
{
    public PositionChange positionChange;
    public PositionChangeNode(Rect rect, string title) : base(rect, title)
    {
    }

    public override void DrawNode(Vector2 offset)
    {

        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect nodeRect = new Rect((rect.position + offset), rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        //base.parentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
}


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;
    public AssetChangeNode(Rect rect, string title) : base(rect, title)
    {
         
    }

    public override void DrawNode(Vector2 offset)
    {

        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect nodeRect = new Rect((rect.position + offset), rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        //base.parentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);
    }
}