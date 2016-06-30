using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{

    public float maxHitpoints = 100;
    public Transform aimPoint;

    private float hitpoints;

    public float Hitpoints
    {
        get
        {
            return hitpoints;
        }
    }

    // Use this for initialization
    void Start()
    {
        hitpoints = maxHitpoints;
        TurretBasePos pos = GetComponent<TurretBasePos>();
        if (pos != null)
        {
            pos.DebugCreateNewTurret();
        }
        if (aimPoint == null)
        {
            aimPoint = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckHitpoints();
    }

    private void CheckHitpoints()
    {
        if (hitpoints < 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            ProcessHit(collision.gameObject.GetComponent<Projectile>(), collision);
        }
    }

    private void ProcessHit(Projectile projectile, Collision collision)
    {
        TakeDamage(projectile.CalculateDamage(collision.relativeVelocity.magnitude));
    }

    private void TakeDamage(float damage)
    {
        //print("Damage: " + damage);
        hitpoints -= damage;
    }
}
