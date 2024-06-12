using UnityEngine;
using Cinemachine;
using System.Collections;

public class SwipeCameraRotation : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;

    [Header("Settings")]
    public float rotationSpeed = 10f;
    public bool canDrag = true;
    public float resetTime = 4f;
    public float resetDuration = 1f;

    private float rotationY;
    private Vector2 startTouchPosition;
    private bool isDragging = false;
    private float inactivityTimer = 0f;
    private Coroutine resetCoroutine;

    void Start()
    {
        rotationY = virtualCamera.transform.localEulerAngles.y;
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (canDrag)
        {
            HandleTouchInput();
            HandleInactivity();
        }
        HandleKeyboardInput();
       
    }

    public void SetCanDrag(bool value) => canDrag = value;

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isDragging = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
            isDragging = true;
            ResetInactivityTimer();
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 touchDelta = touch.position - startTouchPosition;
            UpdateRotation(touchDelta.x);
            startTouchPosition = touch.position;
            ResetInactivityTimer();
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isDragging = false;
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
        rotationY = NormalizeAngle(rotationY);
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
        float targetRotationY = player.eulerAngles.y;

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
}
