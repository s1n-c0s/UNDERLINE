using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiki : MonoBehaviour
{
    private HealthSystem _HealthSystem;

    private void Start()
    {
        _HealthSystem = GetComponent<HealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken") || (other.CompareTag("Enemy")))
        {
            _HealthSystem.TakeDamage(1);
        }
    }
}