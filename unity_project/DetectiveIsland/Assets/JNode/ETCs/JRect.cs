using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
/*
public class JRect : IEquatable<JRect>, IFormattable
{
    private float m_XMin;
    private float m_YMin;
    private float m_Width;
    private float m_Height;

    public static JRect zero => new JRect(0f, 0f, 0f, 0f);

    public float x
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_XMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_XMin = value; }
    }

    public float y
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_YMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_YMin = value; }
    }

    public Vector2 position
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new Vector2(m_XMin, m_YMin); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            m_XMin = value.x;
            m_YMin = value.y;
        }
    }

    public Vector2 center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new Vector2(x + m_Width / 2f, y + m_Height / 2f); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            m_XMin = value.x - m_Width / 2f;
            m_YMin = value.y - m_Height / 2f;
        }
    }

    public Vector2 min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new Vector2(xMin, yMin); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            xMin = value.x;
            yMin = value.y;
        }
    }

    public Vector2 max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new Vector2(xMax, yMax); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            xMax = value.x;
            yMax = value.y;
        }
    }

    public float width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_Width; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_Width = value; }
    }

    public float height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_Height; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_Height = value; }
    }

    public Vector2 size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return new Vector2(m_Width, m_Height); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            m_Width = value.x;
            m_Height = value.y;
        }
    }

    public float xMin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_XMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            float num = xMax;
            m_XMin = value;
            m_Width = num - m_XMin;
        }
    }

    public float yMin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_YMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            float num = yMax;
            m_YMin = value;
            m_Height = num - m_YMin;
        }
    }

    public float xMax
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_Width + m_XMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_Width = value - m_XMin; }
    }

    public float yMax
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_Height + m_YMin; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { m_Height = value - m_YMin; }
    }

    public JRect(float x, float y, float width, float height)
    {
        m_XMin = x;
        m_YMin = y;
        m_Width = width;
        m_Height = height;
    }

    public JRect()
    {
        m_XMin = 0;
        m_YMin = 0;
        m_Width = 0;
        m_Height = 0;
    }
    public JRect(Vector2 position, Vector2 size)
    {
        m_XMin = position.x;
        m_YMin = position.y;
        m_Width = size.x;
        m_Height = size.y;
    }

    public JRect(JRect source)
    {
        m_XMin = source.m_XMin;
        m_YMin = source.m_YMin;
        m_Width = source.m_Width;
        m_Height = source.m_Height;
    }

    public static JRect MinMaxRect(float xmin, float ymin, float xmax, float ymax)
    {
        return new JRect(xmin, ymin, xmax - xmin, ymax - ymin);
    }

    public void Set(float x, float y, float width, float height)
    {
        m_XMin = x;
        m_YMin = y;
        m_Width = width;
        m_Height = height;
    }

    public bool Contains(Vector2 point)
    {
        return point.x >= xMin && point.x < xMax && point.y >= yMin && point.y < yMax;
    }

    public bool Contains(Vector3 point, bool allowInverse = false)
    {
        if (!allowInverse)
        {
            return Contains(point);
        }

        bool flag = (width < 0f && point.x <= xMin && point.x > xMax) || (width >= 0f && point.x >= xMin && point.x < xMax);
        bool flag2 = (height < 0f && point.y <= yMin && point.y > yMax) || (height >= 0f && point.y >= yMin && point.y < yMax);
        return flag && flag2;
    }

    public bool Overlaps(JRect other, bool allowInverse = false)
    {
        JRect rect = this;
        if (allowInverse)
        {
            rect = OrderMinMax(rect);
            other = OrderMinMax(other);
        }

        return rect.Overlaps(other);
    }

    private static JRect OrderMinMax(JRect rect)
    {
        if (rect.xMin > rect.xMax)
        {
            float num = rect.xMin;
            rect.xMin = rect.xMax;
            rect.xMax = num;
        }

        if (rect.yMin > rect.yMax)
        {
            float num = rect.yMin;
            rect.yMin = rect.yMax;
            rect.yMax = num;
        }

        return rect;
    }

    public static bool operator !=(JRect lhs, JRect rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator ==(JRect lhs, JRect rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);
    }

    public override bool Equals(object other)
    {
        if (!(other is JRect))
        {
            return false;
        }

        return Equals((JRect)other);
    }

    public bool Equals(JRect other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && width.Equals(other.width) && height.Equals(other.height);
    }

    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string format)
    {
        return ToString(format, null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "F2";
        }

        if (formatProvider == null)
        {
            formatProvider = CultureInfo.InvariantCulture.NumberFormat;
        }

        return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), width.ToString(format, formatProvider), height.ToString(format, formatProvider));
    }
    public Rect ToRect()
    {
        return new Rect(m_XMin, m_YMin, m_Width, m_Height);
    }


}

*/