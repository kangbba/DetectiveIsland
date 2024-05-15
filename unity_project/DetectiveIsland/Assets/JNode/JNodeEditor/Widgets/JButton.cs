using System;
using UnityEngine;

public class JButton : JImage
{
    private Action _action;

    // Constructor
    public JButton(Vector2 pos, Vector2 size, string title, Action action, JAnchor anchor = JAnchor.Center)
        : base(pos, size, title, anchor)
    {
        _action = action;
    }

    public Action ButtonAction => _action;

    // Method to draw the button with anchor handling
    public override void DrawButton()
    {
        Rect buttonRect = GetRect();

        if (GUI.Button(buttonRect, Title))
        {
            _action.Invoke();
        }
    }
}
