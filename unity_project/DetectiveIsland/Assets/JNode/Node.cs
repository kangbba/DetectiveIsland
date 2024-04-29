using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aroka.ArokaUtils;
using Color = UnityEngine.Color;
using System;
using System.Drawing;

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
        Texture2D circleTexture = Texture.CircleTexture;
        rect = innerCircleRect;
        GUI.color = color;
        GUI.DrawTexture(edgeRect, circleTexture);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.DrawTexture(innerCircleRect, circleTexture);

        GUI.color = Color.white;
    }
}



[System.Serializable]
public abstract class Node
{
    public Vector2 nodeSize;
    public Vector2 position;
    public Vector2 offset;
    public string title;
    public bool isSelected;

    private bool rectNeedsUpdate = true;
    private Rect cachedRect;
    private string id;

    private ConnectingPoint _parentConnectingPoint;
    public ConnectingPoint ParentConnectingPoint
    {
        get => _parentConnectingPoint ??= new ConnectingPoint();
        set => _parentConnectingPoint = value;
    }

    private ConnectingPoint _childConnectingPoint;
    public ConnectingPoint ChildConnectingPoint
    {
        get => _childConnectingPoint ??= new ConnectingPoint();
        set => _childConnectingPoint = value;
    }

    public Rect Rect
    {
        get
        {
            if (rectNeedsUpdate)
            {
                cachedRect = new Rect(position + offset, nodeSize);
                rectNeedsUpdate = false;
            }
            return cachedRect;
        }
    }

    public string ID
    {
        get => id;
        set
        {
            id = value;
            Debug.Log($"Set Guid Node | {title} | {id}");
        }
    }

    public Node(string title)
    {
        this.title = title;
    }

    public void SetGuid()
    {
        ID = Guid.NewGuid().ToString(); 
    }

    public abstract Vector2 CalNodeSize();

    public void UpdateNodePosition(Vector2 newPosition)
    {
        if (position != newPosition)
        {
            position = newPosition;
            rectNeedsUpdate = true;
        }
    }

    public void UpdateNodeSize(Vector2 newSize)
    {
        if (nodeSize != newSize)
        {
            nodeSize = newSize;
            rectNeedsUpdate = true;
        }
    }

    public void UpdateOffset(Vector2 newOffset)
    {
        if (offset != newOffset)
        {

            offset = newOffset;
            rectNeedsUpdate = true;
        }
    }

    public virtual void DrawNode(Vector2 offset)
    {
        UpdateOffset(offset);
    }

    public void DrawNodeLayout(Color color)
    {
        DrawSelectionBox(Rect, color);
        DrawBackground(Rect);
        DrawTitle(Rect, title);
        UpdateNodeSize(CalNodeSize());
    }


    private void DrawSelectionBox(Rect nodeRect, Color representColor)
    {
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
    }

    private void DrawBackground(Rect nodeRect)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);
        GUI.color = Color.white;
    }

    private void DrawTitle(Rect nodeRect, string title)
    {
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);
    }
    public void DrawConnectionPoints(Color representColor, bool parentSide, bool childSide)
    {
        if (parentSide)
            ParentConnectingPoint.DrawSingleConnectionPoint(new Vector2(Rect.x + Rect.width / 2, Rect.y + 12), representColor);
        if (childSide)
            ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(Rect.x + Rect.width / 2, Rect.y + Rect.height - 12), representColor);
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
}


