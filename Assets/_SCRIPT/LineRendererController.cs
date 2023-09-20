/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform playerTransform; // ตำแหน่งของตัวละคร
    public float maxDistance = 5f; // ระยะสูงสุดที่ Line Renderer จะแสดง

    void Update()
    {
        // ดำเนินการเฉพาะเมื่อคลิกเม้าส์
        if (Input.GetMouseButton(0))
        {
            // ดำเนินการเฉพาะเมื่อ Line Renderer มีการเปิดใช้งาน
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }

            // คำนวณระยะทางระหว่างตัวละครกับตำแหน่งเม้าส์
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            float distance = Vector3.Distance(playerTransform.position, mousePosition);

            // จำกัดระยะทางไม่เกิน maxDistance
            if (distance > maxDistance)
            {
                distance = maxDistance;
            }

            // อัพเดทขนาดของ Line Renderer
            lineRenderer.SetPosition(0, playerTransform.position);
            lineRenderer.SetPosition(1, playerTransform.position + playerTransform.forward * distance);
        }
        else
        {
            // ปิด Line Renderer เมื่อไม่คลิกเม้าส์
            lineRenderer.enabled = false;
        }
    }
}*/

/*using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    [Header("****Trail Renderer****")]
    public TrailRenderer trailRenderer;
    public float trailTime = 1f; // Trail display time

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation to prevent rolling
        startPosition = transform.position;
        targetPosition = startPosition;

        // Get a reference to the player's collider
        playerCollider = GetComponent<Collider>();

        // Disable Trail Renderer initially
        trailRenderer.emitting = false;
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

                    // Start creating the trail
                    trailRenderer.Clear();
                    trailRenderer.emitting = true;
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

                // Calculate and display the trajectory
                CalculateAndDisplayTrajectory();
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

                // Stop creating the trail after a certain time
                StartCoroutine(StopTrail());
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

    void CalculateAndDisplayTrajectory()
    {
      
        Vector3 origin = launchPoint.position; // ตำแหน่งเริ่มต้น
        Vector3 velocity = launchPoint.forward * launchSpeed; // ความเร็วเริ่มต้น
        Vector3 currentPosition = origin;
        float timeStep = timeIntervalinPoints; // ช่วงเวลาระหว่างจุด
        int index = 0;

    // ล้างเส้นทาง trail ก่อนเริ่มใหม่
     trailRenderer.Clear();

        while (index < linePoints)
        {
            // คำนวณตำแหน่งใหม่ของจุด
            currentPosition += velocity * timeStep;

            // นำตำแหน่งใหม่ไปแสดงบน Trail Renderer
            trailRenderer.AddPosition(currentPosition);

            // หยุดหากเส้นทางเกินระยะทางที่ต้องการแสดง
            if (Vector3.Distance(origin, currentPosition) > maxDistance)
            {
                break;
            }

            // อัปเดตช่วงเวลา
            timeStep += timeIntervalinPoints;
            index++;

        }
    }

    IEnumerator StopTrail()
    {
        yield return new WaitForSeconds(trailTime);

        // Stop creating the trail
        trailRenderer.emitting = false;
    }
}*/