using System;
using Cinemachine.Utility;
using Lean.Pool;
using UnityEngine;

public class SlashBlade : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int damage;
    [SerializeField] private float lifetime = 3f; // Assign a default lifetime if not set in the Inspector

    [Header("VFX")]
    [SerializeField] private ParticleSystem fx_hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (rb.velocity.magnitude > 0.01f)
        {
            afterShoot();
        }
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
            //PlayHitEffect();
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
            ParticleSystem hitEffect = Instantiate(fx_hit, transform.position, Quaternion.identity);
            hitEffect.Play();
            Destroy(hitEffect.gameObject, hitEffect.main.duration);
        }
    }
}