using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class HealBook : MonoBehaviour
{
    // Reference to the HealthSystem component of the player
    private HealthSystem playerHealth;

    private void Start()
    {
        // Find and assign the HealthSystem component of the player
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            // Heal the player by 1
            playerHealth.Heal(1);
            
            // Destroy the heal book after it's been picked up
            LeanPool.Despawn(gameObject);
        }
    }
}