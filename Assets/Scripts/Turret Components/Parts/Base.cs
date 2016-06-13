using UnityEngine;
using System.Collections;

public class Base : TurretComponent
{
    // ==== DEBUG ====
    internal override void BuildRandom()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i];
            TurretComponent[] components = TurretComponentLibrary.instance.GetComponentsMatching(slot, typeof (Motor));
            if (components.Length > 0)
            {
                TurretComponent baseComponent = components[UnityEngine.Random.Range(0, components.Length)];
                InstantiateComponentAt(baseComponent, i);
                slot.component.BuildRandom();
            }
        }
    }
}
