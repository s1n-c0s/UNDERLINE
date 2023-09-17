using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDimyoMovement : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player's transform
    private bool isPlayerHit = false;
    private bool isPlayerMoving = false; // Variable to track player movement
    private float currentRotation = 0f; // Current rotation of the enemy
    public float rotationSpeed = 90f; // Rotation speed in degrees per second

    void Start()
    {
        // Find the player's transform by tag (you can also assign it manually in the Inspector)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isPlayerHit)
        {
            // Check if the player is moving
            CheckPlayerMovement();

            // Rotate the enemy if the player is moving
            if (isPlayerMoving)
            {
                // Rotate the enemy continuously by adding rotationSpeed * Time.deltaTime
                currentRotation += rotationSpeed * Time.deltaTime;

                // If the enemy has completed a full rotation (360 degrees), reset the rotation
                if (currentRotation >= 360f)
                {
                    currentRotation -= 360f;
                }

                // Apply the rotation
                transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
            }
        }
    }

    // Function to handle player hits
    public void HitPlayer()
    {
        isPlayerHit = true;
        // Here, you can implement logic to handle what happens when the player is hit by the enemy.
    }

    // Function to check if the player is moving
    void CheckPlayerMovement()
    {
        // You can customize this logic based on how you determine if the player is moving in your game.
        // For example, you can check the player's velocity or any other relevant factors.
        // Here, we assume that the player is moving if their velocity magnitude is greater than 0.1.
        if (playerTransform.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            isPlayerMoving = true;
        }
        else
        {
            isPlayerMoving = false;
        }
    }
}