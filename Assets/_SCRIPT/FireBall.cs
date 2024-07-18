using System;
using Cinemachine.Utility;
using Lean.Pool;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float lifetime = 3f; // Assign a default lifetime if not set in the Inspector
    [SerializeField] private bool isArea = false;
    
    /*[Header("VFX")]
    [SerializeField] private ParticleSystem fx_hit;*/

    [Header("Burn Effect")]
    [SerializeField] private float burnDuration = 2f;
    [SerializeField] private int burnDamagePerSecond = 1;

    private void Start()
    {
        if (isArea == false && rb == null)
        {
            rb = GetComponent<Rigidbody>(); 
        }
    }

    private void LateUpdate()
    {
        if (!isArea && rb.velocity.magnitude > 0.01f)
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
        StatusManager statusManager = other.GetComponent<StatusManager>();

        if (healthSystem != null)
        {
            if (statusManager != null)
            {
                statusManager.ApplyStatus(StatusManager.Status.Burn, true, burnDamagePerSecond, burnDuration);
            }
            /*else
            {
                healthSystem.TakeDamage(2);
            }*/
            /*PlayHitEffect();*/
            if (!isArea)
            {
                LeanPool.Despawn(gameObject);
            }
        }
        
        /*if (other.CompareTag("Wall"))
        {
            PlayHitEffect();
            LeanPool.Despawn(gameObject);
        }*/
    }

    /*private void PlayHitEffect()
    {
        if (fx_hit != null)
        {
            ParticleSystem hitEffect = LeanPool.Spawn(fx_hit, transform.position, Quaternion.identity);
            hitEffect.Play();
            LeanPool.Despawn(hitEffect.gameObject, hitEffect.main.duration);
        }
    }*/
}
