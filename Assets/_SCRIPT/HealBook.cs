using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class HealBook : MonoBehaviour
{
    // Reference to the HealthSystem component of the player
    private HealthSystem playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player") && !other.GetComponent<ShadowLife>())
        {
            playerHealth = other.GetComponent<HealthSystem>();
            // Heal the player by 1
            playerHealth.Heal(1);
            
            // Destroy the heal book after it's been picked up
            LeanPool.Despawn(gameObject);
        }
    }
}