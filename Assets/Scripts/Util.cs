using UnityEngine;
using System;

public class Util {
    public static E GetRandomItem<E>(E[] list)
    {
        if (list != null && list.Length > 0)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }
        return default(E);
    }

    public static E[] Subset<E>(E[] array, int index)
    {
        E[] newArray = new E[array.Length - index];
        Array.ConstrainedCopy(array, index, newArray, 0, array.Length - index);
        return newArray;
    }
}
