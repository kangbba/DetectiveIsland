using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aroka.ArokaUtils;
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
public class Node
{
    public Vector2 nodeSize;
    public Vector2 position;
    public Vector2 offset;

    public Rect Rect
    {
        get
        {
            return new Rect(position + offset, nodeSize);
        }
    }

    public void UpdateNodePosition(Vector2 position)
    {
        this.position = position;
    }

    public void UpdateNodeSize(Vector2 size)
    {
        nodeSize = size;
    }
    public void UpdateNodeSizeX(int i)
    {
        nodeSize.x = i;
    }
    public void UpdateNodeSizeY(int i)
    {
        nodeSize.y = i;
    }

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

    public void DrawConnectionPoints(Color representColor, bool parentSide, bool childSide)
    {
        if (parentSide)
            ParentConnectingPoint.DrawSingleConnectionPoint(new Vector2(Rect.x + Rect.width / 2, Rect.y + 12), representColor);
        if (childSide)
            ChildConnectingPoint.DrawSingleConnectionPoint(new Vector2(Rect.x + Rect.width / 2, Rect.y + Rect.height - 12), representColor);
    }


    public Node(string title)
    { 
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
        this.offset = offset;
    }
}


