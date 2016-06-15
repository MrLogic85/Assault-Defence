using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (TurretComponent))]
public class Battery : MonoBehaviour {
    public float capacity = 100;

    private float storedPower = 0;
    private TurretComponent component;

    void Start()
    {
        component = GetComponent<TurretComponent>();
        if (component.parentComponent != null)
        {
            component.GetTurret().RegisterBatery(this);
        }
        else
        {
            component.onFittedEvent += Fitted;
        }
    }

    private void Fitted()
    {
        component.GetTurret().RegisterBatery(this);
    }

    /**
     * Stores power and returns the excess it could not store
     */
    internal float StorePower(float power)
    {
        if (power + storedPower > capacity)
        {
            float excess = power + storedPower - capacity;
            storedPower = capacity;
            return excess;
        }
        else
        {
            storedPower += power;
            return 0;
        }
    }

    internal float GetPower()
    {
        return storedPower;
    }

    /**
     * Consumes power and returns the ammount it could not consume
     */
    internal float ConsumePower(float power)
    {
        if (power > storedPower)
        {
            float rest = power - storedPower;
            storedPower = 0;
            return rest;
        }
        else
        {
            storedPower -= power;
            return 0;
        }
    }
}
