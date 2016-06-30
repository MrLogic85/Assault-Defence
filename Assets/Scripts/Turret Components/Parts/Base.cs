using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (Turret))]
public class Base : TurretComponent
{
    private Turret turret;

    internal override bool GetTurret(out Turret _turret)
    {
        if (turret == null)
        {
            turret = GetComponent<Turret>();
        }
        _turret = turret;
        return turret != null;
    }

    private bool hasPrintedWeight = false;
    void Update()
    {
        if (!hasPrintedWeight)
        {
            print("Total weight " + GetChildTotalWeight());
            hasPrintedWeight = true;
        }
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
