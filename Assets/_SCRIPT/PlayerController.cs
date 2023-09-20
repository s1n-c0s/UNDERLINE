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
    public LineRenderer _lineRenderer;
    public Transform launchPoint;
    public float launchSpeed = 10f;
    public int linePoints = 20;
    public float timeIntervalinPoints = 0.1f;
    public float maxDistance = 170f;

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

                // Draw the trajectory
                DrawTrajectory();
                _lineRenderer.enabled = true;
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

            //Destroy(_lineRenderer);
            _lineRenderer.enabled = false;
        }
    }
    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity1 = launchSpeed * launchPoint.forward;
        Vector3 startVelocity2 = -launchSpeed * launchPoint.forward *6/2; // ปรับให้แกนปลายของ Line Renderer พุ่งออกไปข้างหน้า

        _lineRenderer.positionCount = linePoints * 2;

        float timeInterval = timeIntervalinPoints / linePoints;
        float time = 0;

        for (int i = 0; i < linePoints; i++)
        {
            var x1 = origin.x + startVelocity1.x * time;
            var y1 = origin.y + startVelocity1.y * time - 0.5f * Mathf.Abs(Physics.gravity.y) * time * time;
            var z1 = origin.z + startVelocity1.z * time;

            var x2 = origin.x + startVelocity2.x * time;
            var y2 = origin.y + startVelocity2.y * time - 0.5f * Mathf.Abs(Physics.gravity.y) * time * time;
            var z2 = origin.z + startVelocity2.z * time;

            Vector3 point1 = new Vector3(x1, y1, z1);
            Vector3 point2 = new Vector3(x2, y2, z2);

            // คำนวณระยะทางจากตัวละครไปยังจุดปลายทางที่ผู้เล่นลากเม้าส์
            float distanceToTarget = Vector3.Distance(origin, targetPosition);
            float distanceToPoint1 = Vector3.Distance(origin, point1);
            float distanceToPoint2 = Vector3.Distance(origin, point2);

            /*if (distanceToPoint1 > distanceToTarget)
            {
                // ตัดส่วนที่เกินระยะทางที่ผู้เล่นลากเม้าส์ออก
                point1 = origin + (point1 - origin).normalized * distanceToTarget;
            }*/

            if (distanceToPoint2 > distanceToTarget)
            {
                // ตัดส่วนที่เกินระยะทางที่ผู้เล่นลากเม้าส์ออก
                point2 = origin + (point2 - origin).normalized * distanceToTarget;
                
            }

            _lineRenderer.SetPosition(i * 2, point1);
            _lineRenderer.SetPosition(i * 2 + 1, point2);

            time += timeInterval;
        }
    }
}