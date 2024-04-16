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
    public Line line;

    public LineNode(Vector2 position, float width, float height, string title)
        : base(position, width, height, title)
    {
        line = new Line("defaultEmotion", "defaultSentence");  // Initialize with default values
    }

   public override void DrawNode()
    {
        base.DrawNode();  // Draw the basic node box and start the layout area

        // Make sure to start a new GUI area inside the node's rectangle for additional fields
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 20, rect.width - 20, rect.height - 40));

        if (line != null)
        {
            EditorGUI.BeginChangeCheck();  // Track changes to handle undo/redo properly

            // These fields are now inside the BeginArea/EndArea block
            string newEmotionID = EditorGUILayout.TextField("Emotion ID", line.EmotionID);
            string newSentence = EditorGUILayout.TextField("Sentence", line.Sentence);

            if (EditorGUI.EndChangeCheck())
            {
               // Undo.RecordObject(this, "Edit Line Node");  // Record changes for undo
                line = new(newEmotionID, newSentence);
            }
        }

        GUILayout.EndArea();  // End the GUI area for node content
    }

}

public class DialogueNode : Node
{
    public Dialogue dialogue;

    public DialogueNode(Vector2 position, float width, float height, string title)
        : base(position, width, height, title)
    {
    }
}