using UnityEngine;

public class WhiteBallController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isAiming = false;
    private bool isMoving = false;

    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f; // Adjust this threshold for when the ball is considered stopped

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation to prevent rolling
        startPosition = transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Start aiming when the left mouse button is pressed
                isAiming = true;
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
            }

            if (Input.GetMouseButtonUp(0) && isAiming)
            {
                // Shoot the ball in the opposite direction when the left mouse button is released
                Vector3 shootDirection = startPosition - targetPosition;
                shootDirection.y = 0; // Ensure the shot stays in the same plane as the table

                float power = Mathf.Clamp(shootDirection.magnitude * powerMultiplier, 0f, maxPower);

                rb.AddForce(shootDirection.normalized * power, ForceMode.Impulse);

                // Update the start position to the current position
                startPosition = transform.position;

                // Reset the aiming flag
                isAiming = false;

                // Indicate that the ball is moving
                isMoving = true;

                // Allow rotation again
                rb.freezeRotation = false;
            }
        }
        else
        {
            // Check if the ball has stopped moving
            if (rb.velocity.magnitude < stopThreshold)
            {
                isMoving = false;
                rb.freezeRotation = true; // Freeze rotation again
            }
        }
    }
}