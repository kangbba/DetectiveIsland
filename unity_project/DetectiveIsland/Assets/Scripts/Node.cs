using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class Node
{
    public Rect rect;
    public string title;
    public Vector2 dragOffset;

    public Node(Vector2 position, float width, float height, string title)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.title = title;
    }
}