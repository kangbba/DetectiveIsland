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
    public Rect DetectorRect;
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

    public void UpdateNodeDetect()
    {

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
    public float LineWidth => NodeSizeX;
    public float LineHeight = 70;
    public float LineVeritcalDist = 5;

    public static float NodeSizeX
    {
        get
        {
            return 600;
        }
    }
    public static float NodeSizeY = 150;
    public DialogueNode(float x, float y, string title) : base(new Rect(x, y, NodeSizeX, NodeSizeY), title)
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
        GUIContent labelContent = new GUIContent("Character Type:");
        Vector2 labelSize_CharacterName = labelStyle.CalcSize(labelContent);
        GUI.Label(new Rect(nodeTotalRect.x, nodeTotalRect.y + 60, labelSize_CharacterName.x, 20), labelContent, labelStyle);
        // Character Name Input Field
        dialogue.CharacterID = GUI.TextField(new Rect(nodeTotalRect.x + labelSize_CharacterName.x + 5, nodeTotalRect.y + 60, nodeTotalRect.width - labelSize_CharacterName.x - 10, 20), dialogue.CharacterID);
    }


    private void DrawAddLineButton(Rect nodeRect)
    {
        float buttonYPosition = nodeRect.y + 105 + (dialogue.Lines.Count * (LineHeight + LineVeritcalDist)) ;
        Rect buttonRect = new Rect(
            nodeRect.x + (nodeRect.width - LineWidth) / 2,
            buttonYPosition,
            LineWidth,
            LineHeight
        );

        if (GUI.Button(buttonRect, "Add Line"))
            OnPlusButtonClicked();
    }

    private void DrawLines(Rect nodeRect)
    {
        float yPos = nodeRect.y + 105;
        for (int i = 0; i < dialogue.Lines.Count; i++)
        {
            Rect lineRect = new Rect(nodeRect.x + (nodeRect.width - LineWidth) / 2, yPos + (i * (LineHeight + LineVeritcalDist)), LineWidth, LineHeight);
             GUIStyle lineLabelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            fontSize = 10,
            normal = { textColor = Color.yellow, background = DefaultTexture.GetFlatTexture(Color.yellow * Color.gray * Color.gray) }
        };
            GUI.Box(lineRect, "Line", lineLabelStyle);
            DrawLineContent(i, lineRect);
        }
    }

    private void DrawLineContent(int index, Rect lineRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        GUIStyle textFieldStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white, background = DefaultTexture.GetFlatTexture(Color.gray * 0.25f) },
            fontSize = 10
        };
        float initialLineContentsOffsetY = 20;

        Line line = dialogue.Lines[index];
        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + initialLineContentsOffsetY, 80, 20), "Emotion ID:", labelStyle);
        line.EmotionID = EditorGUI.TextField(new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY, 150, 20), line.EmotionID);

        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + +initialLineContentsOffsetY+ 25, 80, 20), "Sentence:", labelStyle);
        float calLength = NodeService.CalStringVisualSize(textFieldStyle, line.Sentence).x;
        line.Sentence = EditorGUI.TextField(new Rect(lineRect.x + 85 , lineRect.y + initialLineContentsOffsetY + 25, 50 + calLength, 20), line.Sentence, textFieldStyle);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.dialogueColor;

        base.DrawNode(offset);
        Vector2 rectSizeScaling = rect.size + new Vector2(0, (dialogue.Lines.Count + 1) * (LineHeight + LineVeritcalDist));

        Rect nodeTotalRect = new Rect((rect.position + offset ) , rectSizeScaling);
        DetectorRect = nodeTotalRect;
        DrawBackground(nodeTotalRect);
        DrawTitle(nodeTotalRect);
        DrawCharacterType(nodeTotalRect);
        DrawAddLineButton(nodeTotalRect);
        DrawLines(nodeTotalRect);

        GUI.color = Color.white;
        base.ParentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rectSizeScaling.y - 12) + offset, representColor);
    }
   

    private void OnPlusButtonClicked()
    {
        dialogue.Lines.Add(new Line("Smile", ""));
    }
}

public static class DefaultTexture
{
    public static Texture2D GetFlatTexture(Color c)
    {
        Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Icons/WhiteTexture.png");
        texture2D.SetPixel(0, 0, c);
        texture2D.Apply();
        return texture2D;
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
public class PositionInitNode : Node
{
    public PositionInit positionInit;
    public PositionInitNode(Rect rect, string title) : base(rect, title)
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