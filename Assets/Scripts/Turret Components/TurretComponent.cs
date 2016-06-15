using UnityEngine;
using System.Collections;
using System;

public class TurretComponent : MonoBehaviour
{

    [Header("Slots")]
    public Fitting fitting;
    public Slot[] slots;

    internal Action onFittedEvent;

    internal Enemy target;
    internal TurretComponent parentComponent;

    public void Start()
    {
        //FindComponents();
    }

    private void FindComponents()
    {
        foreach (Slot slot in slots)
        {
            foreach (Transform child in slot.transform)
            {
                slot.component = child.gameObject.GetComponent<TurretComponent>();
                if (slot.component != null)
                {
                    slot.component.FittedOn(this, slot.armament);
                    break;
                }
            }
        }
    }

    internal virtual Turret GetTurret()
    {
        return parentComponent.GetTurret();
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
        if (onFittedEvent != null)
        {
            onFittedEvent.Invoke();
        }
    }

    internal virtual bool GetAimOffsetWeight(out float offset)
    {
        bool hasTarget = false;
        offset = 0;
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
        return hasTarget;
    }

    internal void InstantiateComponentAt(TurretComponent comp, Slot slot)
    {
        TurretComponent instance = Instantiate(comp, slot.transform.position, slot.transform.rotation) as TurretComponent;
        instance.transform.parent = slot.transform;
        slot.component = instance;
        instance.FittedOn(this, slot.armament);
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
                    TurretComponent baseComponent = components[UnityEngine.Random.Range(0, components.Length)];
                    InstantiateComponentAt(baseComponent, slot);
                    slot.component.BuildRandom();
                }
            }
        }
    }
}
