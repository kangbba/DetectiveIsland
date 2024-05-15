using UnityEngine;

public class JImage
{
    protected Vector2 _pos;
    protected Vector2 _size;
    protected string _title;
    protected JAnchor _anchor;
    protected TextAnchor _textAnchor;

    public JImage(Vector2 pos, Vector2 size, string title, JAnchor anchor = JAnchor.Center, TextAnchor textAnchor = TextAnchor.MiddleCenter)
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
        Rect rect;
        switch (_anchor)
        {
            case JAnchor.TopLeft:
                rect = new Rect(_pos.x, _pos.y, _size.x, _size.y);
                break;
            case JAnchor.TopRight:
                rect = new Rect(_pos.x - _size.x, _pos.y, _size.x, _size.y);
                break;
            case JAnchor.BottomLeft:
                rect = new Rect(_pos.x, _pos.y - _size.y, _size.x, _size.y);
                break;
            case JAnchor.BottomRight:
                rect = new Rect(_pos.x - _size.x, _pos.y - _size.y, _size.x, _size.y);
                break;
            case JAnchor.CenterTop:
                rect = new Rect(_pos.x - _size.x * 0.5f, _pos.y, _size.x, _size.y);
                break;
            case JAnchor.CenterBottom:
                rect = new Rect(_pos.x - _size.x * 0.5f, _pos.y - _size.y, _size.x, _size.y);
                break;
            case JAnchor.CenterLeft:
                rect = new Rect(_pos.x, _pos.y - _size.y * 0.5f, _size.x, _size.y);
                break;
            case JAnchor.CenterRight:
                rect = new Rect(_pos.x - _size.x, _pos.y - _size.y * 0.5f, _size.x, _size.y);
                break;
            case JAnchor.Center:
            default:
                rect = new Rect(_pos.x - _size.x * 0.5f, _pos.y - _size.y * 0.5f, _size.x, _size.y);
                break;
        }
        return rect;
    }

    public virtual void DrawButton()
    {
        Rect buttonRect = GetRect();

        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = _textAnchor
        };

        GUI.Label(buttonRect, Title, centeredStyle);
    }
}
