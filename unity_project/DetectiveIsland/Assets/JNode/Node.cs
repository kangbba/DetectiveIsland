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

    public DialogueNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.dialogueColor;

        base.DrawNode(offset );


        Rect nodeRect = new Rect((rect.position + offset ) , rect.size );

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }
         
        GUI.color = NodeColor.dialogueColor;
        GUI.Box(nodeRect.AdjustSize(2,2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);

        GUI.color = Color.white;

        base.ParentConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + 12) + offset, representColor);
        base.ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(rect.x + rect.width / 2, rect.y + rect.height - 12) + offset, representColor);

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
public class ScenarioBeginNode : Node
{
    public ScenarioBeginNode(Rect rect, string title) : base(rect, title)
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