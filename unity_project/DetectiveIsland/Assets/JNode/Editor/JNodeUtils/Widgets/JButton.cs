using System;
using UnityEngine;

public class JButton : JTextRect
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
    public override void Draw()
    {
        Rect buttonRect = GetRect();

        GUIStyle gUIStyle = new GUIStyle(GUI.skin.button)
        {
            normal = { background = Texture2D.grayTexture, textColor = Color.white },
            alignment = TextAnchor.MiddleCenter,
            fontSize = 9,
            wordWrap = false,
            clipping = TextClipping.Overflow,
            fontStyle = FontStyle.Bold
        };

        if (GUI.Button(buttonRect, Title, gUIStyle))
        {
            _action.Invoke();
        }
    }
}