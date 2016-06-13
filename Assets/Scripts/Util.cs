using UnityEngine;
using System.Collections;

public class Util {
    public static E GetRandomItem<E>(E[] list)
    {
        if (list != null && list.Length > 0)
        {
            return list[Random.Range(0, list.Length)];
        }
        return default(E);
    }
}
