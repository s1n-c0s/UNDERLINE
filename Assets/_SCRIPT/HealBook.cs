using System;
using UnityEngine;

public class HealBook : MonoBehaviour
{
    // Reference to the HealthSystem component of the player
    [SerializeField] private int healPoint = 1;
    [SerializeField] private float healDuration = 2f; // Duration for the heal effect

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player") && !other.GetComponent<ShadowLife>())
        {
            var playerHealth = other.GetComponent<HealthSystem>();
            
            if (playerHealth != null)
            {
                // Heal the player by specified heal points
                playerHealth.Heal(healPoint, healDuration);
            }
            
            // Destroy the HealBook object after use
            Destroy(gameObject);
        }
    }
}