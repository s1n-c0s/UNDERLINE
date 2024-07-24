using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class SwipeCameraRotation : MonoBehaviour
{
    [Header("Settings")]
    private static bool canSwipe;
    public bool CanSwipe
    {
        get => canSwipe;
        set => canSwipe = value;
    }

    public float rotationSpeed = 10f;
    public float swipeSensitivity = 1;
    public float inactivityThreshold = 4f;
    public float resetDuration = 2f;
    public float countdownBeforeReset = 0f;

    public CinemachineVirtualCamera virtualCamera;
    public Transform player;

    private float currentRotationY;
    private Vector2 initialTouchPosition;
    private bool isSwiping = false;
    private float inactivityTimer = 0f;
    private Coroutine resetCoroutine;
    private Coroutine countdownCoroutine;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Find initial player and set follow target
        player = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.Follow = player;

        currentRotationY = virtualCamera.transform.localEulerAngles.y;
        canSwipe = true;

        // Subscribe to player switch event
        PlayerSwitcher.OnPlayerSwitch += OnPlayerSwitch;
    }

    void OnDestroy()
    {
        // Unsubscribe from player switch event
        PlayerSwitcher.OnPlayerSwitch -= OnPlayerSwitch;
    }

    void Update()
    {
        if (canSwipe)
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
                    StartSwipe(touch.position);
                    break;
                case TouchPhase.Moved:
                    if (isSwiping)
                        UpdateRotation(touch.position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndSwipe();
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            HandleKeyboardInput();
        }
    }

    void StartSwipe(Vector2 position)
    {
        initialTouchPosition = position;
        isSwiping = true;
        ResetInactivityTimer();
    }

    void UpdateRotation(Vector2 currentPosition)
    {
        Vector2 touchDelta = currentPosition - initialTouchPosition;
        float deltaRotation = touchDelta.x * swipeSensitivity * rotationSpeed * Time.deltaTime;
        currentRotationY += deltaRotation;
        currentRotationY = NormalizeAngle(currentRotationY);
        ApplyCameraRotation();
        initialTouchPosition = currentPosition;
        ResetInactivityTimer();
    }

    void EndSwipe()
    {
        isSwiping = false;
    }

    void HandleKeyboardInput()
    {
        float deltaRotation = Input.GetKey(KeyCode.Q) ? rotationSpeed * 10f : -rotationSpeed * 10f; // Increase rotation speed for keyboard input
        deltaRotation *= Time.deltaTime;
        currentRotationY += deltaRotation;
        currentRotationY = NormalizeAngle(currentRotationY);
        ApplyCameraRotation();
        ResetInactivityTimer();
    }

    void HandleInactivity()
    {
        inactivityTimer += Time.deltaTime;
        if (inactivityTimer >= inactivityThreshold && resetCoroutine == null && countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        float countdown = countdownBeforeReset;
        while (countdown > 0f)
        {
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }
        resetCoroutine = StartCoroutine(SmoothResetRotation());
        countdownCoroutine = null;
    }

    IEnumerator SmoothResetRotation()
    {
        float initialRotationY = currentRotationY;
        float targetRotationY = GetNearestAngle(player.eulerAngles.y);

        float elapsedTime = 0f;
        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;
            currentRotationY = Mathf.Lerp(initialRotationY, targetRotationY, Mathf.SmoothStep(0f, 1f, t));
            ApplyCameraRotation();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentRotationY = targetRotationY;
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
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
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
        float currentAngle = currentRotationY;
        float normalizedTargetAngle = NormalizeAngle(targetAngle);
        float angleDifference = Mathf.DeltaAngle(currentAngle, normalizedTargetAngle);
        float nearestAngle = currentAngle + angleDifference;
        return nearestAngle;
    }

    void ApplyCameraRotation()
    {
        Vector3 currentRotation = virtualCamera.transform.localEulerAngles;
        currentRotation.y = currentRotationY;
        virtualCamera.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void OnPlayerSwitch(GameObject newPlayer)
    {
        player = newPlayer.transform;
        virtualCamera.Follow = player;

        // Immediately reset rotation to match the new player
        currentRotationY = virtualCamera.transform.localEulerAngles.y;
        ResetInactivityTimer();
    }
}
