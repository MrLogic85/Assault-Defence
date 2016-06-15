using UnityEngine;
using System;

public class Weapon : TurretComponent
{
    [Header ("Weapon")]
    public ProjectileSpawn[] projectileSpawn;
    public Projectile projectilePrefab;
    public AudioClip[] fireSounds;
    public float flashTime = 0.1f;
    public float rateOfFire = 2;
    public float projectileSpeed = 20;
    public float projectileRange = 20;
    public float recoil = 0.2f;
    [Range (0, 1)]
    public float recoilResetTime = 0.1f;
    [Range(0, 90)]
    public float spreadAngle = 0;

    private static string[] layerMask = new string[] { "Ground", "Enemy", "Turret" };

    private float nextShotTime;
    
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
                Vector3 spawnPos;
                Vector3 spawnForward;
                GetSpawnPosAndDirection(out spawnPos, out spawnForward);
                Ray ray = new Ray(spawnPos, spawnForward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, projectileRange, LayerMask.GetMask(layerMask))
                    && hit.collider.GetComponent<Enemy>() != null)
                {
                    nextShotTime = Time.time + 1 / rateOfFire;
                    ShootProjectile();
                }
            }
        }
    }

    private void GetSpawnPosAndDirection(out Vector3 spawnPos, out Vector3 spawnForward)
    {
        spawnPos = new Vector3();
        spawnForward = new Vector3();
        for (int i = 0; i < projectileSpawn.Length; i++)
        {
            spawnPos += projectileSpawn[i].point.transform.position;
            spawnForward += projectileSpawn[i].point.transform.forward;
        }
        spawnPos /= projectileSpawn.Length;
        spawnForward /= projectileSpawn.Length;
    }

    private void ShootProjectile()
    {
        // Fire
        for (int i = 0; i < projectileSpawn.Length; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawn[i].point.position, projectileSpawn[i].point.rotation) as Projectile;
            projectile.SetSpeed(projectileSpeed);
            projectile.SetRange(projectileRange);

            // Show muzzle flash
            GameObject flash = Util.GetRandomItem(projectileSpawn[i].muzzleFlash);
            if (flash != null)
            {
                flash.SetActive(true);
            }
        }
        Invoke("DectivateFlash", flashTime);

        // Play sound
        if (fireSounds != null)
        {
            AudioManager.Instance.PlaySoundAt(Util.GetRandomItem(fireSounds), transform.position);
        }

        // Recoil
        transform.position -= transform.forward * recoil;
    }

    private void DectivateFlash()
    {
        for (int i = 0; i < projectileSpawn.Length; i++)
        {
            for (int j = 0; j < projectileSpawn[i].muzzleFlash.Length; j++)
            {
                projectileSpawn[i].muzzleFlash[j].SetActive(false);
            }
        }
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
            Vector3 spawnPos, spawnForward;
            GetSpawnPosAndDirection(out spawnPos, out spawnForward);
            Vector3 targetDir = targetPos - spawnPos;

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

    internal override bool GetAimOffsetWeight(out float offset)
    {
        bool baseHasTarget = base.GetAimOffsetWeight(out offset);
        if (HasTarget())
        {
            Vector3 spawnPos, spawnForward;
            GetSpawnPosAndDirection(out spawnPos, out spawnForward);
            offset += Vector3.Angle(spawnForward, GetAimDirection()) * CalculateDamagePotential();
            return true;
        }
        return baseHasTarget;
    }

    public float CalculateDamagePotential()
    {
        if (nextShotTime > Time.time)
        {
            return projectilePrefab.CalculateDamage(projectileSpeed) * projectileSpawn.Length * rateOfFire * (1f / rateOfFire - nextShotTime + Time.time) * rateOfFire;
        }
        return projectilePrefab.CalculateDamage(projectileSpeed) * projectileSpawn.Length * rateOfFire;
    }

    [System.Serializable]
    public struct ProjectileSpawn
    {
        public Transform point;
        public GameObject[] muzzleFlash;
    }
}
