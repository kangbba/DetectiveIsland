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
            Debug.LogError("Null 값을 넣었다");
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
        else if (valueType.IsEnum)
        {
            fieldStyle = new GUIStyle(EditorStyles.popup);
            return EditorGUI.EnumPopup(fieldRect, (Enum)value, fieldStyle);
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
}
