using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Aroka.ArokaUtils;
using static TreeEditor.TreeEditorHelper;
using UnityEngine.UIElements;


[System.Serializable]
public class Node
{
    public Rect rect;
    public string title;
    public bool isSelected;
  

    public Node(Rect rect, string title)
    {
        this.rect = rect;
        this.title = title;
    }

    public void Select()
    {
        Debug.Log(isSelected);
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public virtual void DrawNode(Vector2 offset)
    {
     

    }
}


public static class NodeColor
{
    public static Color nodeBackgroundColor = new Color(0.09803922f, 0.09803922f, 0.09803922f,1f);
    public static Color selectedColor = Color.cyan;
    public static Color dialogueColor = Color.red;

}

public static class NodeGuiService
{
    public static Rect AdjustSize(this Rect rect, float width, float height)
    {
        Rect adjustedRect = rect;
        adjustedRect.x -= width * 0.5f;
        adjustedRect.y -= height * 0.5f;
        adjustedRect.width += width;
        adjustedRect.height += height;
        return adjustedRect;
    }

}

[System.Serializable]
public class DialogueNode : Node
{
    public Dialogue dialogue;

    public DialogueNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset )
    {
        base.DrawNode(offset);
        Rect nodeRect = new Rect((rect.position + offset) , rect.size);

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = NodeColor.dialogueColor;
        GUI.Box(nodeRect.AdjustSize(2,2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect, title, titleGS);


        GUI.color = Color.white;

    }
}


[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;
    public ChoiceSetNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
    }
}

[System.Serializable]
public class ItemDemandNode : Node
{
    public ItemDemand itemDemand;
    public ItemDemandNode(Rect rect, string title) : base(rect, title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
    }
}


[System.Serializable]
public class PositionChangeNode : Node
{
    public PositionChange positionChange;
    public PositionChangeNode(Rect rect, string title) : base(rect, title)
    {
    }

    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
    }
}


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;
    public AssetChangeNode(Rect rect, string title) : base(rect, title)
    {
         
    }

    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
    }
}