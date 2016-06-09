using UnityEngine;
using System.Collections;
using System;

public class Weapon : TurretComponent
{
    [Header ("Weapon")]
    public Transform[] projectileSpawn;
    public Projectile projectilePrefab;
    public float rateOfFire = 2;
    public float projectileSpeed = 20;
    public float projectileRange = 20;
    public float recoil = 0.2f;
    [Range (0, 1)]
    public float recoilResetTime = 0.1f;
    [Range(0, 90)]
    public float spreadAngle = 0;
    [Range(0, 90)]
    public float aimPrecisionFire = 5;

    private float nextShotTime;

    private Enemy target;
    private Vector3 recoilVel;

    // Update is called once per frame
    void Update()
    {
        Shoot();
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilVel, recoilResetTime / rateOfFire);
    }

    private void Shoot()
    {
        if (nextShotTime < Time.time)
        {
            if (HasTarget())
            {
                if (Vector3.Angle(transform.forward, GetAimDirection()) < spreadAngle + aimPrecisionFire)
                {
                    nextShotTime = Time.time + 1 / rateOfFire;
                    ShootProjectile();
                }
            }
        }
    }

    public override void SetTarget(Enemy enemy)
    {
        base.SetTarget(enemy);
        target = enemy;
    }

    private void ShootProjectile()
    {
        for (int i = 0; i < projectileSpawn.Length; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
            projectile.SetSpeed(projectileSpeed);
            projectile.SetRange(projectileRange);
        }

        // Recoil
        transform.position -= transform.forward * recoil;
    }

    private bool HasTarget()
    {
        // Do we have a target?
        if (target == null)
        {
            return false;
        }

        // Can we reach the target?
        Vector3 dist = transform.position - target.transform.position;
        dist.y = 0;
        if (dist.sqrMagnitude > Math.Pow(projectileRange, 2))
        {
            return false;
        }

        return true;
    }

    private Vector3 GetAimDirection()
    {
        if (HasTarget())
        {
            Vector3 targetPos = target.aimPoint.transform.position;
            Vector3 targetDir = targetPos - transform.position;
            if (projectilePrefab.GetComponent<Rigidbody>() == null
                || !projectilePrefab.GetComponent<Rigidbody>().useGravity)
            {
                return  targetDir;
            }
            else
            {
                // Calculate projectile trajectory
                float g = Physics.gravity.y;
                float dy = targetDir.y;
                float dx2 = targetDir.x * targetDir.x + targetDir.z * targetDir.z;
                float v2 = projectileSpeed * projectileSpeed;
                float p = -2 * dy / g - v2 / g * g;
                float q = (dy * dy + dx2) / g * g;
                float T1 = (-p - (float) Math.Sqrt(p * p - 4 * q)) / 2;
                float T2 = (-p + (float)Math.Sqrt(p * p - 4 * q)) / 2;
                float T = (Math.Min(T1, T2) > 0 ? Math.Min(T1, T2) : Math.Max(T1, T2));
                float t = (float) Math.Sqrt(T);
                float vx = targetDir.x / t;
                float vz = targetDir.z / t;
                float vy = targetDir.y / t - t * g;
                return new Vector3(vx, vy, vz);
            }
        }
        return Vector3.forward;
    }

    internal override Boolean GetAimOffsetWeight(out float offset)
    {
        Boolean baseHasTarget = base.GetAimOffsetWeight(out offset);
        if (HasTarget())
        {
            offset += Vector3.Angle(transform.forward, GetAimDirection()) * CalculateDamagePotential();
            return true;
        }
        return baseHasTarget;
    }

    internal float CalculateDamagePotential()
    {
        return projectilePrefab.CalculateDamage(projectileSpeed) * projectileSpawn.Length * rateOfFire;
    }
}
