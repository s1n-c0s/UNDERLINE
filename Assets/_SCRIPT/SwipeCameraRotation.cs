using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class SwipeCameraRotation : MonoBehaviour
{
    [Header("Settings")]
    private static bool canDrag;
    public bool iscanDrag
    {
        get => canDrag;
        set => canDrag = value;
    }

    public float rotationSpeed = 10f;
    public float touchSensitivity = 100f;
    public float resetTime = 3f;
    public float resetDuration = 2f;

    public CinemachineVirtualCamera virtualCamera;
    public Transform player;
    
    private float rotationY;
    private Vector2 startTouchPosition;
    private bool isDragging = false;
    private float inactivityTimer = 0f;
    private Coroutine resetCoroutine;

    private bool isGameEnd = false; // Flag to indicate game over state

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.Follow = player;

        rotationY = virtualCamera.transform.localEulerAngles.y;
        canDrag = true;
    }

    private void OnEnable()
    {
        // Subscribe to player switch event
        PlayerSwitcher.OnPlayerSwitch += OnPlayerSwitch;
        // Subscribe to game over event
        GameManager.OnGameEnd += OnGameEnd;
    }

    void OnDisable()
    {
        PlayerSwitcher.OnPlayerSwitch -= OnPlayerSwitch;
        GameManager.OnGameEnd -= OnGameEnd;
    }

    void Update()
    {
        if (isGameEnd) return; // Skip updates if the game is over

        if (canDrag)
        {
            HandleInput();
            HandleInactivity();
        }
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartTouchInput(touch.position);
                    break;
                case TouchPhase.Moved:
                    if (isDragging)
                        UpdateRotation(touch.position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndTouchInput();
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            HandleKeyboardInput();
        }
    }

    void StartTouchInput(Vector2 position)
    {
        startTouchPosition = position;
        isDragging = true;
        ResetInactivityTimer();
    }

    void UpdateRotation(Vector2 currentPosition)
    {
        Vector2 touchDelta = currentPosition - startTouchPosition;
        float delta = touchDelta.x * touchSensitivity * rotationSpeed * Time.deltaTime;
        rotationY += delta;
        rotationY = NormalizeAngle(rotationY);
        ApplyCameraRotation();
        startTouchPosition = currentPosition;
        ResetInactivityTimer();
    }

    void EndTouchInput()
    {
        isDragging = false;
    }

    void HandleKeyboardInput()
    {
        float delta = Input.GetKey(KeyCode.Q) ? rotationSpeed * 10f : -rotationSpeed * 10f; // Increase rotation speed for keyboard input
        delta *= Time.deltaTime;
        rotationY += delta;
        rotationY = NormalizeAngle(rotationY);
        ApplyCameraRotation();
        ResetInactivityTimer();
    }

    void HandleInactivity()
    {
        inactivityTimer += Time.deltaTime;
        if (inactivityTimer >= resetTime && resetCoroutine == null)
        {
            resetCoroutine = StartCoroutine(SmoothResetRotation());
        }
    }

    IEnumerator SmoothResetRotation()
    {
        float initialRotationY = rotationY;
        float targetRotationY = GetNearestAngle(player.eulerAngles.y);

        float elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;
            rotationY = Mathf.Lerp(initialRotationY, targetRotationY, Mathf.SmoothStep(0f, 1f, t));
            ApplyCameraRotation();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rotationY = targetRotationY;
        ApplyCameraRotation();
        resetCoroutine = null;
        ResetInactivityTimer();
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

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
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

    float GetNearestAngle(float targetAngle)
    {
        float currentAngle = rotationY;
        float normalizedTargetAngle = NormalizeAngle(targetAngle);
        float difference = Mathf.DeltaAngle(currentAngle, normalizedTargetAngle);
        float nearestAngle = currentAngle + difference;
        return nearestAngle;
    }

    void ApplyCameraRotation()
    {
        Vector3 currentRotation = virtualCamera.transform.localEulerAngles;
        currentRotation.y = rotationY;
        virtualCamera.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void OnPlayerSwitch(GameObject newPlayer)
    {
        player = newPlayer.transform;
        virtualCamera.Follow = player;

        // Immediately reset rotation to match the new player
        rotationY = virtualCamera.transform.localEulerAngles.y;
        ResetInactivityTimer();
    }

    void OnGameEnd()
    {
        isGameEnd = true; // Set flag when the game is over
    }
}
