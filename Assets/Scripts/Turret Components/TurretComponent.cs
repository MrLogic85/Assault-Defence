﻿using UnityEngine;
using System.Collections;
using System;

public class TurretComponent : MonoBehaviour {

    [Header ("Slots")]
    public Fitting fitting;
    public Slot[] slots;

    public virtual void SetTarget(Enemy target)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i];
            if (slot.component != null)
            {
                slot.component.SetTarget(target);
            }
        }
    }

    internal virtual void FittedOn(Armament armament) { }

    internal virtual Boolean GetAimOffsetWeight(out float offset)
    {
        Boolean hasTarget = false;
        offset = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].component != null)
            {
                float getOffset;
                if (slots[i].component.GetAimOffsetWeight(out getOffset))
                {
                    offset += getOffset;
                    hasTarget = true;
                }
            }
        }
        return hasTarget;
    }

    private void InstantiateComponentAt(TurretComponent comp, int i)
    {
        TurretComponent instance = Instantiate(comp, slots[i].transform.position, slots[i].transform.rotation) as TurretComponent;
        instance.transform.parent = slots[i].transform;
        slots[i].component = instance;
        instance.FittedOn(slots[i].armament);
    }

    // ==== DEBUG ====
    internal void BuildRandom()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i];
            TurretComponent[] components = TurretComponentLibrary.instance.GetComponentsMatching(slot);
            if (components.Length > 0)
            {
                TurretComponent baseComponent = components[UnityEngine.Random.Range(0, components.Length)];
                InstantiateComponentAt(baseComponent, i);
                slot.component.BuildRandom();
            }
        }
    }
}
