using UnityEngine;
using UnityEditor;
using System;

public class JInputField : JTextRect
{
    public object Value;
    private float _labelWidth;
    private float _fieldWidth;

    public JInputField(object value, Vector2 pos, string title, float labelWidth, float fieldWidth, float height, JAnchor anchor = JAnchor.Center, TextAnchor textAnchor = TextAnchor.MiddleCenter) 
        : base(pos, new Vector2(labelWidth + fieldWidth, height), title, anchor, textAnchor)
    {
        Value = value;
        _labelWidth = labelWidth;
        _fieldWidth = fieldWidth;
    }

    public override void Draw()
    {
        base.Draw(); // Draw the base JImage

        // Draw custom field
        Value = DrawAndGetValue(); // Draw the field
        // You can handle the new value here, e.g., update the _value field with it
    }

    public object DrawAndGetValue()
    {
        base.Draw(); // Draw the base JImage
        if (Value == null)
        {
            return Value;
        }
        Type valueType = Value.GetType();

        Vector2 position = GetRect().position;
        Rect labelRect = new Rect(position.x, position.y, _labelWidth, GetRect().height);
        Rect fieldRect = new Rect(position.x + _labelWidth, position.y, _fieldWidth, GetRect().height);

        EditorGUI.PrefixLabel(labelRect, new GUIContent(Title));

        if (valueType == typeof(string))
        {
            return EditorGUI.TextField(fieldRect, (string)Value);
        }
        else if (valueType == typeof(int))
        {
            return EditorGUI.IntField(fieldRect, (int)Value);
        }
        else if (valueType == typeof(float))
        {
            return EditorGUI.FloatField(fieldRect, (float)Value);
        }
        else if (valueType == typeof(bool))
        {
            return EditorGUI.Toggle(fieldRect, (bool)Value);
        }
        else if (valueType == typeof(long))
        {
            return EditorGUI.LongField(fieldRect, (long)Value);
        }
        else
        {
            return Value;
        }
    }
}
