using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aroka.ArokaUtils;
using Color = UnityEngine.Color;
using System;
using System.Drawing;
using UnityEditor.Experimental.GraphView;


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

    private Rect _nodeRect = new Rect(0, 0, 400, 300);
    private Rect _titleRect = new Rect(0, 0, 200, 20);
    private Rect _stringRect = new Rect(0, 0, 200, 20);
    private Rect _intRect = new Rect(0, 0, 200, 20);
    private Rect _floatRect = new Rect(0, 0, 200, 20);
    private Rect _boolRect = new Rect(0, 0, 200, 20);
    private Rect _longStringRect = new Rect(0, 0, 400, 60);

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
        Type valueType = value.GetType(); // value의 타입을 얻음
        Vector2 position = _nodeRect.position - localPosInNode;

        if (valueType == typeof(string))
        {
            _stringRect.position = position;
            _stringRect.size = new Vector2(width, height);
            return EditorGUI.TextArea(_stringRect, (string)value);
        }
        else if (valueType == typeof(int))
        {
            _intRect.position = position;
            _intRect.size = new Vector2(width, height);
            return EditorGUI.IntField(_intRect, title, (int)value);
        }
        else if (valueType == typeof(float))
        {
            _floatRect.position = position;
            _floatRect.size = new Vector2(width, height);
            return EditorGUI.FloatField(_floatRect, title, (float)value);
        }
        else if (valueType == typeof(bool))
        {
            _boolRect.position = position;
            _boolRect.size = new Vector2(width, height);
            return EditorGUI.Toggle(_boolRect, title, (bool)value);
        }
        else if (valueType == typeof(long))
        {
            _longStringRect.position = position;
            _longStringRect.size = new Vector2(width, height);
            return EditorGUI.LongField(_longStringRect, title, (long)value); // EditorGUI에는 LongField가 기본적으로 없음, 사용자 정의 필요
        }
        else
        {
            // 처리할 수 없는 타입일 경우
            return value; // 그대로 반환하거나 예외를 발생시킬 수 있음
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
            GUI.color = NodeColor.selectedColor;
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

        GUI.color = NodeColor.nodeBackgroundColor;
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


