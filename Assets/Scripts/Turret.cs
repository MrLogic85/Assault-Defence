using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (TurretComponent))]
public class Turret : MonoBehaviour
{
    private TurretComponent turretComponent;
    private Enemy target;
    private List<Battery> batteries = new List<Battery>();

    // Use this for initialization
    void Start ()
    {
        turretComponent = GetComponent<TurretComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        target = FindClosestEnemy();
        turretComponent.SetTarget(target);
    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0)
        {
            return null;
        }

        float minDist = float.MaxValue;
        Enemy closestEnemy = null;
        for (int i = 0; i < enemies.Length; i++)
        {
            float dist = (transform.position - enemies[i].transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = enemies[i];
            }
        }

        return closestEnemy;
    }

    // ==== POWER ====

    internal void RegisterBatery(Battery battery)
    {
        print("Battery registered");
        batteries.Add(battery);
    }

    internal void ProducePower(float power)
    {
        if (GetStoredPower() < GetPowerCapacity())
        {
            foreach (Battery battery in batteries)
            {
                power = battery.StorePower(power);
                if (power <= 0)
                {
                    break;
                }
            }
            print("Producing power. Power stored " + GetStoredPower() + "/" + GetPowerCapacity());
        }
    }

    internal float GetStoredPower()
    {
        float toalStoredPower = 0;
        foreach (Battery battery in batteries)
        {
            toalStoredPower += battery.GetPower();
        }
        return toalStoredPower;
    }

    internal float GetPowerCapacity()
    {
        float toaltotlaCapacity = 0;
        foreach (Battery battery in batteries)
        {
            toaltotlaCapacity += battery.capacity;
        }
        return toaltotlaCapacity;
    }

    internal bool ConsumePower(float power)
    {
        if (GetStoredPower() >= power)
        {
            foreach (Battery battery in batteries)
            {
                power = battery.ConsumePower(power);
            }
            print("Consuming power. Power stored " + GetStoredPower() + "/" + GetPowerCapacity());
            return true;
        }
        return false;
    }
}
