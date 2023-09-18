using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyDimyoMovement : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player's transform
    private bool isPlayerHit = false;
    private bool isPlayerMoving = false; // Variable to track player movement
    private bool isRotating = false; // Variable to track if the enemy is currently rotating
    private float rotationSpeed = 90f; // Rotation speed in degrees per second
    private float maxRotation = 90f; // Maximum rotation in degrees

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

            // Rotate the enemy if the player is moving and is not currently rotating
            if (isPlayerMoving && !isRotating)
            {
                // Start rotating
                StartCoroutine(RotateEnemyCoroutine());
            }
        }
    }

    // Coroutine to rotate the enemy by 90 degrees
    IEnumerator RotateEnemyCoroutine()
    {
        isRotating = true; // Set the rotating flag to true
        float startRotation = transform.rotation.eulerAngles.y;
        float targetRotation = startRotation + maxRotation;

        while (transform.rotation.eulerAngles.y < targetRotation)
        {
            float newRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
            yield return null;
        }

        isRotating = false; // Set the rotating flag to false when the rotation is complete
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
