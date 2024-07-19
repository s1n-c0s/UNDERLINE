using System;
using Cinemachine.Utility;
using Lean.Pool;
using UnityEngine;

public class SlashBlade : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float lifetime = 3f; // Assign a default lifetime if not set in the Inspector

    [Header("VFX")]
    [SerializeField] private ParticleSystem fx_hit;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            afterShoot();
        }
        lastPosition = transform.position;
    }

    private void afterShoot()
    {
        LeanPool.Despawn(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();

        if (healthSystem != null)
        {
            healthSystem.TakeDamage(damage);
            LeanPool.Despawn(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            PlayHitEffect();
            LeanPool.Despawn(gameObject);
        }
        // Additional handling for other cases can be added here
    }

    private void PlayHitEffect()
    {
        if (fx_hit != null)
        {
            ParticleSystem hitEffect = LeanPool.Spawn(fx_hit, transform.position, Quaternion.identity);
            hitEffect.Play();
            LeanPool.Despawn(hitEffect.gameObject, hitEffect.main.duration);
        }
    }
}