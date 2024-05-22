using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padding
{
    private float _top;
    private float _bottom;
    private float _left;
    private float _right;

    public Padding(float top, float bottom, float left, float right)
    {
        _top = top;
        _bottom = bottom;
        _left = left;
        _right = right;
    }

    public float Top { get => _top; }
    public float Bottom { get => _bottom;  }
    public float Left { get => _left;  }
    public float Right { get => _right; }
}
