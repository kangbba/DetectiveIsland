
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class ConnectingPoint
{
    private Rect _rect;
    private bool _isLineModifying;

    public ConnectingPoint(string nodeID, bool isChildConnectingPoint)
    {   
        NodeID = nodeID;
        IsChildConnectingPoint = isChildConnectingPoint;
    }

    public bool IsChildConnectingPoint ;
    public string NodeID { get ; set; }

    [JsonIgnore] public bool IsLineModifying { get => _isLineModifying; }
    [JsonIgnore] public Rect Rect { get => _rect; }
    
    public void ModifyingStart(bool b){
        _isLineModifying = b;
    }
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

        GUI.color = NodeColors.nodeBackgroundColor;
        GUI.DrawTexture(innerCircleRect, circleTexture);

        GUI.color = Color.white;
    }
}