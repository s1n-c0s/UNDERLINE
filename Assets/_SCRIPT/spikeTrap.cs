using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int Damage = 1;
    [SerializeField] private ParticleSystem fx_hit;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(Damage);
            ParticleSystem fxInit = LeanPool.Spawn(fx_hit, transform.position, quaternion.identity);
            LeanPool.Despawn(fx_hit, 3f);
        }
    }
}