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

        NextConnectingPoint = new ConnectingPoint(nodeID, true);
        PreviousConnectingPoint = new ConnectingPoint(nodeID, false);
    }

    public ConnectingPoint NextConnectingPoint {get; set;}
    public ConnectingPoint PreviousConnectingPoint {get; set;}

    public abstract Vector2 CalNodeSize();

    public abstract Element ToElement();

    public string Title;
    public string NodeID;
    public string NextNodeID ;
    public string ParentNodeID ;

    public bool IsMostParentNode => ParentNodeID == null || ParentNodeID == "";

    public bool IsSelected ;

    public Vector2 lastRectPos;
    public Vector2 lastRectSize;

    [JsonIgnore] public Rect NodeRect { get => _nodeRect; }
    private Rect _nodeRect;

    public void SetNextNodeID(string nextNodeID){
        NextNodeID = nextNodeID;
    }
    public virtual void DrawNode(){    
        DrawBackground(NodeRect);
        DrawTitle(Title);
        
        if(IsSelected){
            DrawHighlight();
        }

        if(IsMostParentNode){
            PreviousConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), Color.white);
            NextConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), Color.white);
        }
    }

    public void RefreshNodeSize(){
        SetNodeRectSize(CalNodeSize());
    }
    public void SetNodeRectSize(Vector2 size){
        _nodeRect.size = size;

        lastRectSize = size;
    }
    
    public void SetRectPos(Vector2 newPos){
        _nodeRect.position = newPos; // 위치 업데이트

        lastRectPos = newPos;
    }
    private void DrawDebugLabel(){
        GUI.Label(NodeRect, NodeID.ToString());
    }
    // 데이터 타입에 따른 CustomField 메서드
    protected object CustomField(string title, object value, Vector2 localPosInNode, float width = 80, float height = 20)
    {
        if (value == null)
        {
            return value;
        }

        Type valueType = value.GetType(); // value의 타입을 얻음
        Vector2 position = _nodeRect.position + localPosInNode;
        float labelWidth = 100; // Label의 너비를 적절히 설정합니다.
        
        // Label과 필드를 각각의 위치에 배치합니다.
        Rect labelRect = new Rect(position.x, position.y, labelWidth, height);
        Rect fieldRect = new Rect(position.x + labelWidth, position.y, width, height);

        EditorGUI.PrefixLabel(labelRect, new GUIContent(title));

        if (valueType == typeof(string))
        {
            return EditorGUI.TextField(fieldRect, (string)value);
        }
        else if (valueType == typeof(int))
        {
            return EditorGUI.IntField(fieldRect, (int)value);
        }
        else if (valueType == typeof(float))
        {
            return EditorGUI.FloatField(fieldRect, (float)value);
        }
        else if (valueType == typeof(bool))
        {
            return EditorGUI.Toggle(fieldRect, (bool)value);
        }
        else if (valueType == typeof(long))
        {
            return EditorGUI.LongField(fieldRect, (long)value);
        }
        else
        {
            // 처리할 수 없는 타입일 경우
            return value; // 그대로 반환하거나 예외를 발생시킬 수 있음
        }
    }


    protected string CustomTextArea(string value, Vector2 localPosInNode, float width = 100, float height = 300)
    {
        Vector2 position = _nodeRect.position + localPosInNode;
        Rect fieldRect = new Rect(position.x, position.y, width, height);

        return EditorGUI.TextArea(fieldRect, value);
    }
    protected void ImagePreview(string filePath, float width, float height)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            // Calculate the aspect ratio
            float aspectRatio = (float)texture.width / texture.height;
            float drawWidth = width;
            float drawHeight = height;

            // Adjust width and height to maintain aspect ratio
            if (width / height > aspectRatio)
            {
                drawWidth = height * aspectRatio;
            }
            else
            {
                drawHeight = width / aspectRatio;
            }

            Rect rect = new Rect(10, 10, drawWidth, drawHeight); // Example position
            GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true);
        }
        else
        {
            Debug.LogError("Failed to load image: " + filePath);
        }
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
        GUI.color = Color.white;
    }

    public void SetSelected(bool b){
        IsSelected = b;
    }

    private void DrawBackground(Rect nodeRect)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        GUI.color = NodeColors.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);
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
    }
    public bool IsMouseOver(Vector2 mousePosition)
    {
        return _nodeRect.Contains(mousePosition); // 마우스 위치가 노드 내부인지 판단
    }

    
}


