
using UnityEngine;

[System.Serializable]
public class ConnectingPoint
{
    private string _nodeID;
    private Rect _rect;

    private bool _isChildConnectingPoint;

    public ConnectingPoint(string nodeID, bool isChildConnectingPoint)
    {   
        _nodeID = nodeID;
        _isChildConnectingPoint = isChildConnectingPoint;
    }
        public Rect Rect { get => _rect;   }
    public bool IsChildConnectingPoint { get => _isChildConnectingPoint; }
    public string NodeID { get => _nodeID; }

    public bool IsContainRect(Vector2 pos)
    {
        return _rect.Contains(pos);
    }
    public void DrawSingleConnectionPoint(Vector2 centerPoint, Color color)
    {
        float innerRadius = 10;  // 내부 원의 반지름
        float edgeThickness = 1;  // 테두리 두께
        float edgeRadius = innerRadius + edgeThickness;  // 테두리 원의 반지름

        Rect edgeRect = new Rect(centerPoint.x - edgeRadius, centerPoint.y - edgeRadius, edgeRadius * 2, edgeRadius * 2);
        Rect innerCircleRect = new Rect(centerPoint.x - innerRadius, centerPoint.y - innerRadius, innerRadius * 2, innerRadius * 2);
        Texture2D circleTexture = Texture.CircleTexture;
        _rect = innerCircleRect;
        GUI.color = color;
        GUI.DrawTexture(edgeRect, circleTexture);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.DrawTexture(innerCircleRect, circleTexture);

        GUI.color = Color.white;
    }
}
