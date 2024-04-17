using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class Node
{
    public Rect rect;
    public string title;

    public Node(Rect rect, string title)
    {
        this.rect = rect;
        this.title = title;
    }

    public virtual void DrawNode(Vector2 offset)
    {
        GUI.Box(new Rect(rect.position + offset, rect.size), title);
    }
}

[System.Serializable]
public class DialogueNode : Node
{
    public DialogueNode(Rect rect, string title) : base(rect, title) { }
    public Dialogue dialogue;
    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
        // 여기에 추가적인 노드 UI 구성 요소들을 그리세요
    }
}







/*
[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;
    public ChoiceSetNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {

    }

}

[System.Serializable]
public class ItemDemandNode : Node
{
    public ItemDemand itemDemand;
    public ItemDemandNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {
    }

}


[System.Serializable]
public class PositionChangeNode : Node
{
    public PositionChange positionChange;
    public PositionChangeNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {
    }
  
}


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;
    public AssetChangeNode(Vector2 position, float width, float height, string title) : base(position, width, height, title)
    {

    }

}

*/