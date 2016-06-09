using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public enum HitType
    {
        Bounce,
        Stick,
        Destroy
    }

    public HitType hitType;
    public ParticleSystem destroyEffect;

    private float speed;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDestroy()
    {
        if (destroyEffect != null)
        {
            ParticleSystem particles =  Instantiate(destroyEffect, transform.position, Quaternion.FromToRotation(Vector3.forward, transform.forward)) as ParticleSystem;
            Destroy(particles.gameObject, particles.startLifetime);
        }
    }

    internal void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
        Rigidbody projBody = GetComponent<Rigidbody>();
        projBody.velocity = projectileSpeed * transform.forward;
    }

    internal void SetRange(float projectileRange)
    {
        Destroy(gameObject, projectileRange / speed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        switch (hitType)
        {
            case HitType.Bounce:
                break;
            case HitType.Stick:
                transform.parent = collision.gameObject.transform;
                GetComponent<Rigidbody>().freezeRotation = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;
            case HitType.Destroy:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public float CalculateDamage(float velocity)
    {
        float mass = GetComponent<Rigidbody>().mass;
        float volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        return velocity * mass * volume * 500;
    }
}
