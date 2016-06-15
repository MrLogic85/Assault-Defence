using System;
using System.Collections;
using UnityEngine;

public class TurretComponentLibrary : MonoBehaviour
{
    public static TurretComponentLibrary instance;

    public Base[] bases;
    [Tooltip ("Components of Armament.Part type")]
    public TurretComponent[] parts;
    [Tooltip("Components of Armament.Utility type")]
    public TurretComponent[] utilities;
    [Tooltip("Components of Armament.Weapon type")]
    public TurretComponent[] weapons;

    public void Awake()
    {
        instance = this;
    }

    internal Base[] GetBases()
    {
        return bases;
    }

    internal TurretComponent[] GetComponentsMatching(Slot slot, Type filter = null)
    {
        TurretComponent[] components = new TurretComponent[parts.Length + utilities.Length + weapons.Length];
        Array.Copy(parts, components, parts.Length);
        Array.Copy(utilities, 0, components, parts.Length, utilities.Length);
        Array.Copy(weapons, 0, components, parts.Length + utilities.Length, weapons.Length);

        ArrayList filteredComponents = new ArrayList();
        for (int i = 0; i < components.Length; i++)
        {
            TurretComponent component = components[i];
            Fitting fitting = component.fitting;
            if (!fitting.size.Fits(slot.size))
            {
                // Make sure the fitting is smaller or equal than the slot
                continue;
            }
            if (!fitting.armament.Fits(slot.armament))
            {
                // Make sure tha armament type can fit in the slot
                continue;
            }
            if (filter != null && component.GetComponent(filter) == null)
            {
                // Make sure the object is of the corect type
                continue;
            }
            if (Debug.isDebugBuild)
            {
                if (filter == null && slot.armament == Armament.Part && fitting.armament != Armament.Part)
                {
                    // If slot is part, then lets select a part to make bigger turrets
                    continue;
                }
            }
            // else
            filteredComponents.Add(component);
        }

        TurretComponent[] objects = new TurretComponent[filteredComponents.Count];
        Array.Copy(filteredComponents.ToArray(), objects, filteredComponents.Count);

        return objects;
    }
}
