using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (TurretComponent))]
public class Battery : MonoBehaviour {
    public float capacity = 100;
    public Renderer glowMaterial;
    public float maxIntensity = 1;

    private float storedPower = 0;
    private TurretComponent component;

    void Start()
    {
        component = GetComponent<TurretComponent>();
        Turret turret;
        if (component.GetTurret(out turret))
        {
            turret.RegisterBatery(this);
        }
        else
        {
            component.onFittedEvent += Fitted;
        }

        UpdateGlow();
    }

    private void Fitted(TurretComponent component, Armament arment)
    {
        Turret turret;
        if (component.GetTurret(out turret))
        {
            turret.RegisterBatery(this);
        }
    }

    private Color originalGlowColor;
    private bool isOrigColorSet;
    private void UpdateGlow()
    {
        if (glowMaterial != null)
        {
            if (!isOrigColorSet)
            {
                originalGlowColor = glowMaterial.material.GetColor("_EmissionColor");
                isOrigColorSet = true;
            }
            glowMaterial.material.SetColor("_EmissionColor", originalGlowColor * (maxIntensity * storedPower / capacity));
        }
    }

    /**
     * Stores power and returns the excess it could not store
     */
    public float StorePower(float power)
    {
        if (storedPower >= capacity)
        {
            return power;
        }

        if (power + storedPower > capacity)
        {
            float excess = power + storedPower - capacity;
            storedPower = capacity;
            UpdateGlow();
            return excess;
        }
        else
        {
            storedPower += power;
            UpdateGlow();
            return 0;
        }
    }

    public float GetPower()
    {
        return storedPower;
    }

    /**
     * Consumes power and returns the ammount it could not consume
     */
    public float ConsumePower(float power)
    {
        if (storedPower <= 0)
        {
            return power;
        }

        if (power > storedPower)
        {
            float rest = power - storedPower;
            storedPower = 0;
            UpdateGlow();
            return rest;
        }
        else
        {
            storedPower -= power;
            UpdateGlow();
            return 0;
        }
    }
}
