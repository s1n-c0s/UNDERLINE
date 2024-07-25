using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Lean.Pool;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private Checkpoint _checkpoint;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool LineisRunning = false;
    private Collider playerCollider;

    public int runsRemaining = 3;
    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f;
    public float runningAngularDamping = 10f;

    public TextMeshProUGUI runsRemainingText;
    public List<SwipeCameraRotation> _swipeCameraRotation;
    public static event Action OnTurnEnd;

    private bool isMoving;
    private bool wasMoving;

    public LineRenderer _lineRenderer;
    public Transform launchPoint;
    public float launchSpeed = 10f;
    public int linePoints = 20;
    public float timeIntervalinPoints = 0.1f;
    public float maxDistance = 170f;

    [Header("VFX")] 
    [SerializeField] private List<GameObject> Prefabfx;
    [SerializeField] private PunchObj _punchObj;
    [SerializeField] private ParticleSystem _CircleRing;

    private Vector3 lastStartPosition;

    private bool isGrounded; // To track if the character is grounded

    private Camera mainCamera;
    private Transform playerTransform;

    private StringBuilder sb = new StringBuilder();
    
    
    private void OnEnable()
    {
       _CircleRing.Play();
    }

    private void OnDisable()
    {
        _CircleRing.Stop();
    }

    void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _checkpoint = GetComponent<Checkpoint>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startPosition = transform.position;
        targetPosition = startPosition;
        playerCollider = GetComponent<Collider>();
        _swipeCameraRotation.Add(FindObjectOfType<SwipeCameraRotation>());

        UpdateRunsRemainingText();

        mainCamera = Camera.main;
        playerTransform = transform;

        Prefabfx[0].SetActive(false);
    }

    void Update()
    {
        UpdateRunsRemainingText();

        if (runsRemaining > 0 && !isMoving)
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

    private void FixedUpdate()
    {
        CheckIfMoving();
        wasMoving = isMoving;
    }

    void HandleRunningInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
            _checkpoint.SetCheckpointPosition();
        }

        if (LineisRunning)
        {
            HandleRunning();
            Prefabfx[0].SetActive(true);
            
            foreach (SwipeCameraRotation camera in _swipeCameraRotation)
            {
                camera.CanSwipe = false;
            }
        }

        if (Input.GetMouseButtonUp(0) && LineisRunning)
        {
            HandleMouseUp();
            Prefabfx[0].SetActive(false);
            
            foreach (SwipeCameraRotation camera in _swipeCameraRotation)
            {
                camera.CanSwipe = true;
            }
        }
    }

    void HandleMouseDown()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (playerCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            LineisRunning = true;
            startPosition = playerTransform.position;
            
            _punchObj.startBounce();
        }
    }

    void HandleRunning()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane tablePlane = new Plane(Vector3.up, startPosition);

        float hitDistance;
        if (tablePlane.Raycast(ray, out hitDistance))
        {
            targetPosition = ray.GetPoint(hitDistance);
        }

        float maxDragRadius = 5f;
        Vector3 playerToTarget = targetPosition - startPosition;
        if (playerToTarget.magnitude > maxDragRadius)
        {
            targetPosition = startPosition + playerToTarget.normalized * maxDragRadius;
        }

        Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.red);

        Vector3 lookDirection = startPosition - targetPosition;
        lookDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        playerTransform.rotation = targetRotation;

        rb.angularDrag = runningAngularDamping;

        DrawTrajectory();
        _lineRenderer.enabled = true;
    }

    void HandleMouseUp()
    {
        Vector3 runDirection = startPosition - targetPosition;
        runDirection.y = 0;

        float maxDragRadius = 5f;
        float normalizedDistance = Mathf.Clamp01(runDirection.magnitude / maxDragRadius);
        float power = Mathf.Lerp(0f, maxPower, normalizedDistance);

        rb.AddForce(runDirection.normalized * power, ForceMode.Impulse);

        LineisRunning = false;
        rb.angularDrag = 0f;
        
        GameObject rushSmoke = LeanPool.Spawn(Prefabfx[2], transform.position + -transform.forward, gameObject.transform.rotation);
        LeanPool.Despawn(rushSmoke, 1f);
    }

    void HandleNotRunning()
    {
        _lineRenderer.enabled = false;
    }

    void CheckIfMoving()
    {
        isMoving = rb.velocity.magnitude > stopThreshold;

        if (!isMoving && wasMoving)
        {
            OnPlayerStop();
        }

        if (wasMoving)
        {
            GameObject runsmoke = LeanPool.Spawn(Prefabfx[1], transform.position, quaternion.identity);
            LeanPool.Despawn(runsmoke, 1f);
        }
     
    }

    void OnPlayerStop()
    {
        DecreaseRunsRemaining();
        OnTurnEnd?.Invoke();
    }

    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity1 = launchSpeed * launchPoint.forward;
        Vector3 startVelocity2 = -launchSpeed * launchPoint.forward * 6 / 2;

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

            float distanceToTarget = Vector3.Distance(origin, targetPosition);
            float distanceToPoint2 = Vector3.Distance(origin, point2);

            if (distanceToPoint2 > distanceToTarget)
            {
                point2 = origin + (point2 - origin).normalized * distanceToTarget;
            }

            _lineRenderer.SetPosition(i * 2, point1);
            _lineRenderer.SetPosition(i * 2 + 1, point2);

            time += timeInterval;
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 1.0f;
        isGrounded = Physics.Raycast(playerTransform.position, Vector3.down, out hit, raycastDistance);
    }

    public void IncreaseRunsRemaining()
    {
        _healthSystem.Heal(1);
        Debug.Log("Cost+1 :" + _healthSystem.GetCurrentHealth());
    }

    public void DecreaseRunsRemaining()
    {
        if (runsRemaining > 0)
        {
            _healthSystem.TakeDamage(1);
            Debug.Log("Cost-1 :" + _healthSystem.GetCurrentHealth());
            UpdateRunsRemainingText();
        }
        else
        {
            // If runs remaining is already zero, you may want to handle this case accordingly
            Debug.Log("No runs remaining!");
        }
    }

    void UpdateRunsRemainingText()
    {
        runsRemaining = _healthSystem.GetCurrentHealth();
        
        /*if (runsRemainingText != null)
        {
            runsRemaining = _healthSystem.GetCurrentHealth();
            
            sb.Clear();
            sb.Append("Runs Remaining: ").Append(runsRemaining);
            runsRemainingText.text = sb.ToString();
        }*/
    }

    public void Respawn()
    {
        transform.position = _checkpoint.GetLastCheckpointPosition();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
