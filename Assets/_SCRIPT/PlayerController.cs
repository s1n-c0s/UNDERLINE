using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("****Player Controller****")]
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isAiming = false;
    private Collider playerCollider; 
    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f; // Adjust this threshold for when the player is considered stopped
    public float aimingAngularDamping = 10f;
    public bool IsMoving { get; private set; }

    [Header("****Line Render****")]
    public LineRenderer lineRenderer;
    public Transform launchPoint;
    public float launchSpeed = 10f;
    public int linePoints = 20;
    public float timeIntervalinPoints = 0.1f;

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
        if (!IsMoving)
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
                
                DrawTrajectory();
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
                IsMoving = true;

                // Restore the angular drag to its default value
                rb.angularDrag = 0f;
            }
        }
        else
        {
            // Check if the player has stopped moving
            if (rb.velocity.magnitude < stopThreshold)
            {
                IsMoving = false;
                rb.freezeRotation = true; // Freeze rotation again
            }
        }
    }

    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity = launchSpeed * launchPoint.forward;

        lineRenderer.positionCount = linePoints;

        float time = 0;

        for (int i = 0; i < linePoints; i++)
        {
            var x = origin.x + startVelocity.x * time;
            var y = origin.y + startVelocity.y * time - 0.5f * Mathf.Abs(Physics.gravity.y) * time * time;
            var z = origin.z + startVelocity.z * time;

            Vector3 point = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, point);

            time += timeIntervalinPoints;
        }
    }
}