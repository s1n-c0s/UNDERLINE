using UnityEngine;
using Cinemachine;
using System.Collections;

public class SwipeCameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 10f;
    public bool canDrag = true; // Variable to enable/disable dragging
    public float resetTime = 4f; // Time after which to reset the rotation
    public float resetDuration = 1f; // Duration over which to smoothly reset

    private float rotationY;
    private Vector2 startTouchPosition;
    private bool isDragging = false;
    private float inactivityTimer = 0f;
    private Coroutine resetCoroutine;

    void Start()
    {
        // Initialize rotationY with the current local Y rotation of the camera
        rotationY = virtualCamera.transform.localEulerAngles.y;
    }

    void Update()
    {
        if (canDrag)
        {
            HandleTouchInput();
        }
        HandleKeyboardInput();
        HandleInactivity();
    }

    public void SetCanDrag(bool value)
    {
        canDrag = value;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isDragging = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                startTouchPosition = touch.position;
                isDragging = true;
                ResetInactivityTimer();
                break;
            case TouchPhase.Moved:
                if (isDragging)
                {
                    Vector2 touchDelta = touch.position - startTouchPosition;
                    UpdateRotation(touchDelta.x);
                    startTouchPosition = touch.position;
                    ResetInactivityTimer();
                }
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isDragging = false;
                break;
        }
    }

    void HandleKeyboardInput()
    {
        bool rotated = false;

        if (Input.GetKey(KeyCode.Q))
        {
            UpdateRotation(-rotationSpeed * Time.deltaTime);
            rotated = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            UpdateRotation(rotationSpeed * Time.deltaTime);
            rotated = true;
        }

        if (rotated)
        {
            ResetInactivityTimer();
        }
    }

    void UpdateRotation(float delta)
    {
        rotationY += delta * rotationSpeed;
        rotationY = NormalizeAngle(rotationY); // Normalize the rotation to stay within -180 to 180 degrees
        ApplyCameraRotation();
    }

    void ApplyCameraRotation()
    {
        Vector3 currentRotation = virtualCamera.transform.localEulerAngles;
        currentRotation.y = rotationY;
        virtualCamera.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void HandleInactivity()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        inactivityTimer += Time.deltaTime;

        if (inactivityTimer >= resetTime && resetCoroutine == null)
        {
            resetCoroutine = StartCoroutine(SmoothResetRotation());
        }
    }

    void ResetInactivityTimer()
    {
        inactivityTimer = 0f;
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
    }

    IEnumerator SmoothResetRotation()
    {
        float elapsedTime = 0f;
        float initialRotationY = rotationY;
        float targetRotationY = player.eulerAngles.y; // Target rotation aligned with player's forward direction

        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Cubic ease-out
            rotationY = Mathf.Lerp(initialRotationY, targetRotationY, easedT);
            ApplyCameraRotation();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotationY = targetRotationY;
        ApplyCameraRotation();
        resetCoroutine = null;
        ResetInactivityTimer();
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        else if (angle < -180f)
        {
            angle += 360f;
        }
        return angle;
    }
}
