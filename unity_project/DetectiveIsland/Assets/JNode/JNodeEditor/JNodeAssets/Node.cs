using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aroka.ArokaUtils;
using Color = UnityEngine.Color;
using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public abstract class Node
{
    public Node(string nodeID, string title, string parentNodeID)
    {
        NodeID = nodeID;

        Title = title;
        ParentNodeID = parentNodeID;

        
        PreviousConnectingPoint = new ConnectingPoint(nodeID, false);
        NextConnectingPoint = new ConnectingPoint(nodeID, true);
    }

    public ConnectingPoint NextConnectingPoint {get; set;}
    public ConnectingPoint PreviousConnectingPoint {get; set;}


    public abstract Element ToElement();



    [JsonIgnore] public abstract float Width { get; set;}
    [JsonIgnore] public abstract float Height { get; set;}

    public bool IsStartNode;
    public string Title;
    public string NodeID;
    public string NextNodeID ;
    public string ParentNodeID ;
    public Vector2 RecentRectPos;
    public Vector2 RecentRectSize;
    public bool IsSelected ;


    [JsonIgnore] public Rect NodeRect { get => _nodeRect; }
    [JsonIgnore] public bool IsMostParentNode => ParentNodeID == null || ParentNodeID == "";

    private Rect _nodeRect;



    public void SetNextNodeID(string nextNodeID){
        NextNodeID = nextNodeID;
    }
    public virtual void DrawNode(){    
        DrawBackground(NodeRect);
        DrawTitle(Title);

        if (IsSelected){
            DrawHighlight();
        }

        if(IsMostParentNode){
            if(!IsStartNode){
                PreviousConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), Color.white);
            }
            NextConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), Color.white);
        }
    }

    public void SetNodeRectSize(Vector2 size){
        _nodeRect.size = size;
        RecentRectSize = _nodeRect.size;
    }
    public void SetNodeRectSize(float width, float height)
    {
        _nodeRect.size = new(width, height);
        RecentRectSize = _nodeRect.size;
    }
    public void AddNodeRectSize(Vector2 size){
        _nodeRect.size += size;
        RecentRectSize = _nodeRect.size;
    }
    
    public void SetRectPos(Vector2 newPos, JAnchor anchor)
    {
        Vector2 anchoredPOS = newPos.GetAnchoredPos(NodeRect.size, anchor);
        _nodeRect.position = anchoredPOS; // 위치 업데이트
        RecentRectPos = anchoredPOS;
    }
    private void DrawDebugLabel(){
        GUI.Label(NodeRect, NodeID.ToString());
    }
    private void DrawHighlight()
    {
        // 선택된 상태일 때 하이라이트 테두리 그리기
        Color highlightColor = Color.cyan; // 하이라이트 색상을 하늘색으로 설정

        // 테두리 색상 설정
        Handles.color = highlightColor;
        Rect highlightRect = new Rect(NodeRect.x - 5, NodeRect.y - 5, NodeRect.width + 10, NodeRect.height + 10);
        
        // 하이라이트 테두리 그리기
        Handles.DrawSolidRectangleWithOutline(highlightRect, Color.clear, highlightColor);

        // 이전 GUI 색상 복원
        Handles.color = Color.white;
        GUI.color = Color.white;
    }


    public void SetSelected(bool b){
        IsSelected = b;
    }
    private void DrawBackground(Rect nodeRect)
    {
        // GUI 스타일 설정
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(2, 2, 2, 2);

        // 배경 박스를 그립니다.
        GUI.color = NodeColors.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);


        // highlightRect와 highlightColor를 사용하여 테두리를 그립니다.
        Rect highlightRect = NodeRect.AdjustSize(2, 2);
        Handles.DrawSolidRectangleWithOutline(highlightRect, Color.clear, Color.white);

        Handles.color = Color.white;
        GUI.color = Color.white;

    }

    private void DrawTitle(string title)
    {
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;

        Vector2 pos = NodeRect.center.ModifiedY(NodeRect.min.y) + new Vector2(-100, 20);
        Rect rect = new Rect(pos.x, pos.y, 200, 100); 

        GUI.Label(rect, title, titleGS);
        GUI.color = Color.white;

    }
    public bool IsMouseOver(Vector2 mousePosition)
    {
        return _nodeRect.Contains(mousePosition); // 마우스 위치가 노드 내부인지 판단
    }

    
}


