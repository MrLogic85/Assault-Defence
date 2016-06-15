using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Turret))]
public class Base : TurretComponent
{
    private Turret turret;

    internal override Turret GetTurret()
    {
        if (turret == null)
        {
            turret = GetComponent<Turret>();
        }
        return turret;
    }

    // ==== DEBUG ====
    internal override void BuildRandom()
    {
        foreach (Slot slot in slots)
        {
            TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
            if (library != null)
            {
                TurretComponent[] components = library.GetComponentsMatching(slot, typeof(Motor));
                if (components.Length > 0)
                {
                    TurretComponent baseComponent = components[UnityEngine.Random.Range(0, components.Length)];
                    InstantiateComponentAt(baseComponent, slot);
                    slot.component.BuildRandom();
                }
            }
        }
    }
}
