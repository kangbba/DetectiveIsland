using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public enum JAnchor
{
    Center,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    CenterTop,
    CenterBottom,
    CenterLeft,
    CenterRight
}


public static class JInterface
{
    public static Rect GetAnchoredRect(this Vector2 pos, Vector2 size, JAnchor anchor)
    {
        Rect rect;
        switch (anchor)
        {
            case JAnchor.TopLeft:
                rect = new Rect(pos.x, pos.y, size.x, size.y);
                break;
            case JAnchor.TopRight:
                rect = new Rect(pos.x - size.x, pos.y, size.x, size.y);
                break;
            case JAnchor.BottomLeft:
                rect = new Rect(pos.x, pos.y - size.y, size.x, size.y);
                break;
            case JAnchor.BottomRight:
                rect = new Rect(pos.x - size.x, pos.y - size.y, size.x, size.y);
                break;
            case JAnchor.CenterTop:
                rect = new Rect(pos.x - size.x * 0.5f, pos.y, size.x, size.y);
                break;
            case JAnchor.CenterBottom:
                rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y, size.x, size.y);
                break;
            case JAnchor.CenterLeft:
                rect = new Rect(pos.x, pos.y - size.y * 0.5f, size.x, size.y);
                break;
            case JAnchor.CenterRight:
                rect = new Rect(pos.x - size.x, pos.y - size.y * 0.5f, size.x, size.y);
                break;
            case JAnchor.Center:
            default:
                rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
                break;
        }
        return rect;
    }
    public static Vector2 GetAnchoredPos(this Vector2 pos, Vector2 size, JAnchor anchor)
    {
        Rect rect = GetAnchoredRect(pos, size, anchor);
        return rect.position;
    }
    public static object SimpleField(string title, object value, Vector2 pos, float labelWidth = 100, float fieldWidth = 80, float fieldHeight = 20)
    {
        if (value == null)
        {
            return value;
        }

        Type valueType = value.GetType(); // value의 타입을 얻음

        // Label과 필드를 각각의 위치에 배치합니다.
        Rect labelRect = new Rect(pos.x, pos.y, labelWidth, fieldHeight);
        Rect fieldRect = new Rect(pos.x + labelWidth, pos.y, fieldWidth, fieldHeight);

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        GUIStyle fieldStyle;

        EditorGUI.PrefixLabel(labelRect, new GUIContent(title), labelStyle);

        if (valueType == typeof(string))
        {
            fieldStyle = new GUIStyle(EditorStyles.textField);
            return EditorGUI.TextField(fieldRect, (string)value, fieldStyle);
        }
        else if (valueType == typeof(int))
        {
            fieldStyle = new GUIStyle(EditorStyles.numberField);
            return EditorGUI.IntField(fieldRect, (int)value, fieldStyle);
        }
        else if (valueType == typeof(float))
        {
            fieldStyle = new GUIStyle(EditorStyles.numberField);
            return EditorGUI.FloatField(fieldRect, (float)value, fieldStyle);
        }
        else if (valueType == typeof(bool))
        {
            fieldStyle = new GUIStyle(EditorStyles.toggle);
            return EditorGUI.Toggle(fieldRect, (bool)value, fieldStyle);
        }
        else if (valueType == typeof(long))
        {
            fieldStyle = new GUIStyle(EditorStyles.numberField);
            return EditorGUI.LongField(fieldRect, (long)value, fieldStyle);
        }
        else
        {
            // 처리할 수 없는 타입일 경우
            return value; // 그대로 반환하거나 예외를 발생시킬 수 있음
        }
    }
    public static string SimpleTextArea(string value, Vector2 pos, float fieldWidth = 80, float height = 20)
    {
        if (value == null)
        {
            return value;
        }
        Rect fieldRect = new Rect(pos.x, pos.y, fieldWidth, height);
        return EditorGUI.TextArea(fieldRect, value);
    }

    public static void AttachDeleteButtons<T>(List<T> nodes, Vector2 btnSize = default) where T : Node
    {
        if(btnSize == default){
            btnSize = Vector2.one * 20f;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            T node = nodes[i];
            if (node is T lineNode)
            {
                string nodeId = lineNode.NodeID;
                JButton deleteBtn = new JButton(
                    pos: new Vector2(lineNode.NodeRect.max.x, lineNode.NodeRect.position.y),
                    size: btnSize,
                    title: "X",
                    anchor: JAnchor.TopRight,
                    action: () => DeleteLineNode(nodes, nodeId)
                );
                deleteBtn.Draw();
            }
        }
    }

    public static void DeleteLineNode<T>(List<T> nodes, string nodeId) where T : Node
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }

        T nodeToDelete = nodes.FirstOrDefault(node => node.NodeID == nodeId);
        if (nodeToDelete != null)
        {
            nodes.Remove(nodeToDelete);
        }
    }
    
    public static void AttachArrowButtons<T>(List<T> nodes, Vector2 btnSize = default) where T : Node
    {
        if(btnSize == default){
            btnSize = Vector2.one * 20f;
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            T node = nodes[i];
            if (node is T lineNode)
            {
                string nodeId = lineNode.NodeID;
                JButton orderUpBtn = new JButton(
                  pos: new Vector2(lineNode.NodeRect.max.x - btnSize.x * 1, lineNode.NodeRect.position.y),
                  size: btnSize,
                  title: "▲",
                  anchor: JAnchor.TopRight,
                  action: () => MoveListOrder(nodes, lineNode.NodeID, -1)
                ); 
                orderUpBtn.Draw();

                JButton orderDownBtn = new JButton(
                  pos: new Vector2(lineNode.NodeRect.max.x - btnSize.x * 2, lineNode.NodeRect.position.y),
                  size: btnSize,
                  title: "▼",
                  anchor: JAnchor.TopRight,
                  action: () => MoveListOrder(nodes, lineNode.NodeID, 1)
                );
                orderDownBtn.Draw();
            }
        }
    }

    public static void MoveListOrder<T>(List<T> nodes, string nodeId, int direction) where T : Node
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }

        int index = nodes.FindIndex(node => node.NodeID == nodeId);
        if (index == -1)
        {
            Debug.LogWarning("Node not found");
            return;
        }

        int newIndex = index + direction;

        // Ensure newIndex is within valid range
        if (newIndex < 0 || newIndex >= nodes.Count)
        {
            Debug.LogWarning("Invalid move direction");
            return;
        }

        // Release focus from the current text area
        GUI.FocusControl(null);

        // Swap the elements to change the order
        T nodeToMove = nodes[index];
        nodes.RemoveAt(index);
        nodes.Insert(newIndex, nodeToMove);
    }

    

}
