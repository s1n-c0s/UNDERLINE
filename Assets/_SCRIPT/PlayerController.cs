﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isAiming = false;
    private bool isMoving = false;
    private Collider playerCollider; // Reference to the player's collider

    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f; // Adjust this threshold for when the player is considered stopped

    // Adjust this value to control the angular damping when aiming
    public float aimingAngularDamping = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation to prevent rolling
        startPosition = transform.position;
        targetPosition = startPosition;

        // Get a reference to the player's collider
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the mouse click is on the player's collider
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (playerCollider.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // Start aiming when the left mouse button is pressed on the player
                    isAiming = true;

                    // Update the start position to the current position
                    startPosition = transform.position;
                }
            }

            if (isAiming)
            {
                // Update the target position based on mouse input
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane tablePlane = new Plane(Vector3.up, startPosition);

                float hitDistance;
                if (tablePlane.Raycast(ray, out hitDistance))
                {
                    targetPosition = ray.GetPoint(hitDistance);
                }

                // Visualize the target position (you can remove this for the final game)
                Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.red);

                // Calculate rotation to face the opposite direction of the cursor
                Vector3 lookDirection = startPosition - targetPosition;
                lookDirection.y = 0f; // Keep the rotation in the horizontal plane
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation;

                // Apply angular damping when aiming to prevent excessive rotation
                rb.angularDrag = aimingAngularDamping;
            }

            if (Input.GetMouseButtonUp(0) && isAiming)
            {
                // Shoot the player in the opposite direction when the left mouse button is released
                Vector3 shootDirection = startPosition - targetPosition;
                shootDirection.y = 0; // Ensure the shot stays in the same plane as the table

                float power = Mathf.Clamp(shootDirection.magnitude * powerMultiplier, 0f, maxPower);

                rb.AddForce(shootDirection.normalized * power, ForceMode.Impulse);

                // Reset the aiming flag
                isAiming = false;

                // Indicate that the player is moving
                isMoving = true;

                // Restore the angular drag to its default value
                rb.angularDrag = 0f;
            }
        }
        else
        {
            // Check if the player has stopped moving
            if (rb.velocity.magnitude < stopThreshold)
            {
                isMoving = false;
                rb.freezeRotation = true; // Freeze rotation again
            }
        }
    }
}
