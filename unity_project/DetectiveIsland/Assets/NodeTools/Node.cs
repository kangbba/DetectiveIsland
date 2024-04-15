using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
public class Node
{
    public Rect rect;
    public string title;
    public Vector2 dragOffset;
    public int ID; // Unique ID for each node

    private static int nextID = 0;

    public Node(Vector2 position, float width, float height, string title)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.title = title;
        ID = nextID++;
    }

    // Function to draw the node GUI and its properties
    public virtual void DrawNode()
    {
        GUI.Box(rect, title);
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 20, rect.width - 20, rect.height - 40));

        // Using reflection to expose fields dynamically
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            object value = field.GetValue(this);
            if (value is string)
            {
                string newValue = EditorGUILayout.TextField(field.Name, (string)value);
                field.SetValue(this, newValue);
            }
            // You can add more types as needed
        }

        GUILayout.EndArea();
    }
}
public class LineNode : Node
{
    public string emotionID;
    public string sentence;

    public LineNode(Vector2 position, float width, float height, string title)
        : base(position, width, height, title)
    {
    }
}
