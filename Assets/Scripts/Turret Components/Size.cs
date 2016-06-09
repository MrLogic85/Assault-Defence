using UnityEngine;
using System.Collections;

public enum Size {
    Small,
    Medium,
    Large,
    XL
}

static class SizeExtension
{
    public static bool Fits(this Size left, Size right)
    {
        return left.value() <= right.value();
    }

    private static int value(this Size size)
    {
        switch (size)
        {
            case Size.Small: return 0;
            case Size.Medium: return 1;
            case Size.Large: return 2;
            case Size.XL: return 3;
            default: return -1;
        }
    }
}
