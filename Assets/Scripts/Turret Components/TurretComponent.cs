using UnityEngine;
using System.Collections.Generic;
using System;

public class TurretComponent : MonoBehaviour
{

    [Header("Slots")]
    public Fitting fitting;
    public Slot[] slots;
    [Header("Characteristics")]
    public float weight;

    internal Enemy target;
    internal TurretComponent parentComponent;
    internal Armament fittedOnArmamanet;

    // ==== EVENTS ====
    internal Action<TurretComponent, Armament> onFittedEvent;
    // In params, current aim offset, has children target. Retuns new aim and if component wants to aim or not
    internal List<Func<float, bool, KeyValuePair<float, bool>>> onComponenGetAimWeightEvent = new List<Func<float, bool, KeyValuePair<float, bool>>>();

    public void Awake()
    {
        // Having declared this methods makes the
        // component available for other scripts Awake method
    }

    internal virtual bool GetTurret(out Turret turret)
    {
        if (parentComponent == null)
        {
            turret = null;
            return false;
        }
        return parentComponent.GetTurret(out turret);
    }

    public virtual void SetTarget(Enemy enemy)
    {
        target = enemy;
        foreach (Slot slot in slots)
        {
            if (slot.component != null)
            {
                slot.component.SetTarget(enemy);
            }
        }
    }

    internal virtual void FittedOn(TurretComponent component, Armament armament)
    {
        parentComponent = component;
        fittedOnArmamanet = armament;
        if (onFittedEvent != null)
        {
            onFittedEvent.Invoke(component, armament);
        }
    }

    internal float GetChildTotalWeight()
    {
        float weight = 0;
        foreach (Slot slot in slots)
        {
            if (slot.component != null)
            {
                weight += slot.component.weight;
                weight += slot.component.GetChildTotalWeight();
            }
        }
        return weight;
    }

    internal virtual bool GetAimOffsetWeight(out float offset)
    {
        bool hasTarget = false;
        offset = 0;
        // Check depth first for aim
        foreach (Slot slot in slots)
        {
            if (slot.component != null)
            {
                float getOffset;
                if (slot.component.GetAimOffsetWeight(out getOffset))
                {
                    offset += getOffset;
                    hasTarget = true;
                }
            }
        }
        // Let each component modify the aim, a motor will for instance reduce the offset and a weapon will add the the offset
        foreach (Func<float, bool, KeyValuePair<float, bool>> aimMethod in onComponenGetAimWeightEvent)
        {
            KeyValuePair<float, bool> result = aimMethod(offset, hasTarget);
            if (result.Value)
            {
                offset = result.Key;
                hasTarget = true;
            }
        }
        return hasTarget;
    }

    internal TurretComponent InstantiateComponentAt(TurretComponent comp, Slot slot)
    {
        TurretComponent instance = Instantiate(comp, slot.transform.position, slot.transform.rotation) as TurretComponent;
        instance.transform.parent = slot.transform;
        slot.component = instance;
        instance.FittedOn(this, slot.armament);
        return instance;
    }

    // ==== DEBUG ====
    internal virtual void BuildRandom()
    {
        foreach (Slot slot in slots)
        {
            TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
            if (library != null)
            {
                TurretComponent[] components = library.GetComponentsMatching(slot);
                if (components.Length > 0)
                {
                    TurretComponent turretComponent = components[UnityEngine.Random.Range(0, components.Length)];
                    InstantiateComponentAt(turretComponent, slot);
                    slot.component.BuildRandom();
                }
            }
        }
    }

    internal virtual void BuildRandom(int[] generateIds)
    {
        TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
        if (library != null)
        {
            for (int i = 0; i < slots.Length && i < generateIds.Length; i++)
            {
                TurretComponent[] components = library.GetComponentsMatching(slots[i]);
                if (components.Length > 0 && generateIds[i] < components.Length && generateIds[i] >= 0)
                {
                    TurretComponent turretComponent = components[generateIds[i]];
                    InstantiateComponentAt(turretComponent, slots[i]);
                    slots[i].component.BuildRandom(Util.Subset(generateIds, slots.Length));
                }
            }
        }
    }
}
