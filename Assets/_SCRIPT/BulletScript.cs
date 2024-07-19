using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool detectDespawn;

    [Header("VFX")] [SerializeField] private GameObject fx_hit; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            if (other.gameObject.GetComponent<HealthSystem>())
            {
                other.GetComponent<HealthSystem>().TakeDamage(damage);
            }
            if (detectDespawn)
            {
                LeanPool.Despawn(gameObject);
            }
            GameObject fx_crash = LeanPool.Spawn(fx_hit, transform.position, Quaternion.identity);
            LeanPool.Despawn(fx_crash, 3f);
        }
    }
}
