using UnityEngine;
using System.Collections;

public enum Armament {
    Part,
    Utility,
    Weapon
}

static class ArmamentExtension
{
    public static bool Fits(this Armament left, Armament right)
    {
        return left.value() <= right.value();
    }

    private static int value(this Armament size)
    {
        switch (size)
        {
            case Armament.Part: return 2;
            case Armament.Utility: return 1;
            case Armament.Weapon: return 0;
            default: return -1;
        }
    }
}
