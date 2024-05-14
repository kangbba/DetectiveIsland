using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aroka.ArokaUtils;
using Color = UnityEngine.Color;
using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using System.IO;


[System.Serializable]
public abstract class Node 
{
    public Node(string title, Node parentNode)
    {
        _id = Guid.NewGuid().ToString();;
        _title = title;
        _parentNode = parentNode;

        _parentConnectingPoint = new ConnectingPoint(_id, false);
        _childConnectingPoint = new ConnectingPoint(_id, true);
    }

    private Vector2 _canvasOffset;
    private bool _isDragging = false; // 드래그 중인지 상태를 추적하는 플래그
    private string _id;
    private string _title;
    private Node _parentNode;
    private Vector2 _localScale;

    private string _nextNodeID = "";
    public bool IsNextNodeExist => _nextNodeID != "";

    public Vector2 Center => _nodeRect.position;

    private bool _isSelected;
    private ConnectingPoint _childConnectingPoint ;
    private ConnectingPoint _parentConnectingPoint ;

    protected Rect _nodeRect = new Rect(0, 0, 400, 300);
    private Rect _titleRect = new Rect(0, 0, 200, 20);

    public abstract Vector2 CalNodeSize();

    public abstract Element ToElement();

    public string ID { get => _id; }
    public Node ParentNode { get => _parentNode;  }
    public Rect NodeRect { get => _nodeRect; }
    public ConnectingPoint ChildConnectingPoint { get => _childConnectingPoint; }
    public ConnectingPoint ParentConnectingPoint { get => _parentConnectingPoint; }
    public string NextNodeID { get => _nextNodeID;  }

    public void SetNextNodeID(string nextNodeID){
        _nextNodeID = nextNodeID;
    }
    public virtual void DrawNode(){    
        DrawBackground(_nodeRect);
        DrawTitle(_title);
    }

    public void SetNodeRectSize(Vector2 size){
        _nodeRect.size = size;
    }
    
    public void SetRectPos(Vector2 newPos){
        _nodeRect.position = newPos; // 위치 업데이트
        _titleRect.position = newPos + new Vector2(_nodeRect.width * .5f - (_titleRect.width * .5f), 15);
    }
    // 데이터 타입에 따른 CustomField 메서드
    protected object CustomField(string title, object value, Vector2 localPosInNode, float width = 80, float height = 20)
    {
        if (value == null)
        {
            Debug.Log("value is null");
            return value;
        }

        Type valueType = value.GetType(); // value의 타입을 얻음
        Vector2 position = _nodeRect.position - localPosInNode;
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

    private void DrawSelectionBox(Rect nodeRect, Color representColor)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (_isSelected)
        {
            GUI.color = NodeColors.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);
    }

    public void SetSelected(bool b){
        _isSelected = b;
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
        GUI.Label(_titleRect, title, titleGS);
    }
    public bool IsMouseOver(Vector2 mousePosition)
    {
        return _nodeRect.Contains(mousePosition); // 마우스 위치가 노드 내부인지 판단
    }
}


