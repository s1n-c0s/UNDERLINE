using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRunning = false;
    private Collider playerCollider;
    private EnemyDetector enemyDetector;

    public int runsRemaining = 3;
    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f;
    public float runningAngularDamping = 10f;

    public TextMeshProUGUI runsRemainingText;
    public bool IsMoving { get; private set; }

    public LineRenderer _lineRenderer;
    public Transform launchPoint;
    public float launchSpeed = 10f;
    public int linePoints = 20;
    public float timeIntervalinPoints = 0.1f;
    public float maxDistance = 170f;

    private Vector3 lastStartPosition;

    private bool isGrounded; // To track if the character is grounded

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startPosition = transform.position;
        targetPosition = startPosition;
        playerCollider = GetComponent<Collider>();
        enemyDetector = GetComponent<EnemyDetector>();
        lastStartPosition = startPosition;
        UpdateRunsRemainingText();
    }

    void Update()
    {
        UpdateRunsRemainingText();
        if (runsRemaining > 0 && !IsMoving)
        {
            HandleRunningInput();
        }
        else
        {
            HandleNotRunning();
        }

        // Check if the character is grounded
        CheckGrounded();
    }

    void HandleRunningInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (isRunning)
        {
            HandleRunning();
        }

        if (Input.GetMouseButtonUp(0) && isRunning)
        {
            HandleMouseUp();
        }
    }

    void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (playerCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            isRunning = true;
            startPosition = transform.position;
        }
    }
    void HandleRunning()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane tablePlane = new Plane(Vector3.up, startPosition);

        float hitDistance;
        if (tablePlane.Raycast(ray, out hitDistance))
        {
            targetPosition = ray.GetPoint(hitDistance);
        }

        // Restrict the target position within a specific radius
        float maxDragRadius = 5f; // Adjust this value to your desired radius
        Vector3 playerToTarget = targetPosition - startPosition;
        if (playerToTarget.magnitude > maxDragRadius)
        {
            targetPosition = startPosition + playerToTarget.normalized * maxDragRadius;
        }

        Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.red);

        Vector3 lookDirection = startPosition - targetPosition;
        lookDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = targetRotation;

        rb.angularDrag = runningAngularDamping;

        DrawTrajectory();
        _lineRenderer.enabled = true;
    }

    void HandleMouseUp()
    {
        Vector3 runDirection = startPosition - targetPosition;
        runDirection.y = 0;

        // Calculate power based on the distance within maxDragRadius
        float maxDragRadius = 5f; // Adjust this value to your desired radius
        float normalizedDistance = Mathf.Clamp01(runDirection.magnitude / maxDragRadius);
        float power = Mathf.Lerp(0f, maxPower, normalizedDistance);

        rb.AddForce(runDirection.normalized * power, ForceMode.Impulse);

        isRunning = false;
        IsMoving = true;
        runsRemaining--;

        if (runsRemaining <= 0)
        {
            isRunning = false;
        }

        rb.angularDrag = 0f;
    }



    void HandleNotRunning()
    {
        if (rb.velocity.magnitude < stopThreshold)
        {
            IsMoving = false;
            rb.freezeRotation = true;
        }

        _lineRenderer.enabled = false;
    }

    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity1 = launchSpeed * launchPoint.forward;
        Vector3 startVelocity2 = -launchSpeed * launchPoint.forward * 6 / 2; // ปรับให้แกนปลายของ Line Renderer พุ่งออกไปข้างหน้า

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

    void CheckGrounded()
    {
        // Perform a raycast downwards to check for ground
        RaycastHit hit;
        float raycastDistance = 1.0f; // Adjust this based on your character's size
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);

        /*// If not grounded, move the character up slightly to keep them grounded
        if (!isGrounded)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + 0.05f; // Adjust this value as needed
            transform.position = newPosition;
        }*/
    }
    public void IncreaseRunsRemaining()
    {
        runsRemaining++;
    }

    public void DecreaseRunsRemaining(int amount)
    {
        runsRemaining -= amount;
    }

    void UpdateRunsRemainingText()
    {
        // Check if runsRemainingText and playerController are not null
        if (runsRemainingText != null )
        {
            // Update the TextMeshPro text with the value of runsRemaining
            runsRemainingText.text = "Runs Remaining: " + runsRemaining.ToString();
        }
    }

    public void Respawn()
    {
        transform.position = lastStartPosition;
    }
}
