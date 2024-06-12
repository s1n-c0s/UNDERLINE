using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeTrap : MonoBehaviour
{
    [SerializeField] private int Damage = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthSystem>().TakeDamage(Damage);
        }
        
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<HealthSystem>().TakeDamage(Damage);
        }
    }
}
