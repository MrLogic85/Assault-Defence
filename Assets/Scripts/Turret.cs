using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (TurretComponent))]
public class Turret : MonoBehaviour
{

    private TurretComponent turretComponent;
    private Enemy target;

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
}
