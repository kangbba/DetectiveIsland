using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;


[System.Serializable]
public class Node
{
    [System.NonSerialized]
    public Rect rect;
    [System.NonSerialized]
    public string title;
    [System.NonSerialized]
    public Vector2 dragOffset;
    [System.NonSerialized]
    public int ID; // Unique ID for each node

    private static int nextID = 0;

    public Node(Vector2 position, float width, float height, string title )
    {
        rect = new Rect(position.x , position.y , width, height);
        this.title = title;
        ID = nextID++;
    }

    // Function to draw the node GUI and its properties
    public virtual void DrawNode()
    {
        // Draw the node box without any title text
        GUI.Box(rect, "");

        // Start an area inside the node for additional GUI elements
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 20, rect.width - 20, rect.height - 40));

        // Using reflection to expose fields dynamically for editing
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            object value = field.GetValue(this);
            if (value is string)
            {
                string newValue = EditorGUILayout.TextField(field.Name, (string)value);
                field.SetValue(this, newValue);
            }
            // Additional field types can be added here as needed
        }
        // End the area inside the node
        GUILayout.EndArea();
    }
}


[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;
    public ChoiceSetNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {

    }

    public override void DrawNode()
    {
        base.DrawNode();
    }
}

[System.Serializable]
public class ItemDemandNode : Node
{
    public ItemDemand itemDemand;
    public ItemDemandNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {
    }

    public override void DrawNode()
    {
        base.DrawNode();
    }
}


[System.Serializable]
public class PositionChangeNode : Node
{
    public PositionChange positionChange;
    public PositionChangeNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {
    }
    public override void DrawNode()
    {
        base.DrawNode();
    }
}


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;
    public AssetChangeNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {

    }

    public override void DrawNode()
    {
        base.DrawNode();  
    }
}


[System.Serializable]
public class DialogueNode : Node
{
    public Dialogue dialogue;
    public string name;
    public DialogueNode(Vector2 position, float width, float height, string title)
        : base(position, width, height, title)
    {

    }
    public override void DrawNode()
    {
        base.DrawNode();  // Draw the basic node box and start the layout area

        // Make sure to start a new GUI area inside the node's rectangle for additional fields
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 20, rect.width - 20, rect.height - 40));

        if (dialogue != null)
        {
            EditorGUI.BeginChangeCheck();  // Track changes to handle undo/redo properly

            // These fields are now inside the BeginArea/EndArea block
            string newEmotionID = EditorGUILayout.TextField("Emotion ID","??");
            string newSentence = EditorGUILayout.TextField("Sentence", "??");

            if (EditorGUI.EndChangeCheck())
            {
                // Undo.RecordObject(this, "Edit Line Node");  // Record changes for undo
                dialogue = new(newEmotionID,null);
            }
        }

        GUILayout.EndArea();  // End the GUI area for node content
    }

}