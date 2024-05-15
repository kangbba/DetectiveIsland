using UnityEngine;

public class JTextRect
{
    protected Vector2 _pos;
    protected Vector2 _size;
    protected string _title;
    protected JAnchor _anchor;
    protected TextAnchor _textAnchor;

    public JTextRect(Vector2 pos, Vector2 size, string title, JAnchor anchor = JAnchor.Center, TextAnchor textAnchor = TextAnchor.MiddleCenter)
    {
        _pos = pos;
        _size = size;
        _title = title;
        _anchor = anchor;
        _textAnchor = textAnchor;
    }

    public Vector2 Pos => _pos;
    public Vector2 Size => _size;
    public JAnchor Anchor => _anchor;
    public string Title => _title;
    public TextAnchor TextAnchor => _textAnchor;

    public Rect GetRect()
    {
        return JInterface.GetAnchoredRect(_pos, _size, _anchor);
    }

    public virtual void Draw()
    {
        Rect buttonRect = GetRect();

        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = _textAnchor
        };

        GUI.Label(buttonRect, Title, centeredStyle);
    }
}
