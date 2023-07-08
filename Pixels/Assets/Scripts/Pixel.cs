using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pixel
{
    public PixelTypes type;
}

public struct PixelTypes
{
    public Color color;

    public static bool operator ==(PixelTypes p1, PixelTypes p2)
    {
        return p1.Equals(p2);
    }
    public static bool operator !=(PixelTypes p1, PixelTypes p2)
    {
        return !p1.Equals(p2);
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}