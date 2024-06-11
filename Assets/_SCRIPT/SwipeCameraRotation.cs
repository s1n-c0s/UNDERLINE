using UnityEngine;
using Cinemachine;

public class SwipeCameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float rotationSpeed = 10f;
    public bool canDrag = true; // Variable to enable/disable dragging

    private float rotationY;
    private Vector2 startTouchPosition;
    private bool isDragging = false;

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
                break;
            case TouchPhase.Moved:
                if (isDragging)
                {
                    Vector2 touchDelta = touch.position - startTouchPosition;
                    UpdateRotation(touchDelta.x);
                    startTouchPosition = touch.position;
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
        if (Input.GetKey(KeyCode.Q))
        {
            UpdateRotation(-rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            UpdateRotation(rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateRotation(float delta)
    {
        rotationY += delta * rotationSpeed;
        ApplyCameraRotation();
    }

    void ApplyCameraRotation()
    {
        Vector3 currentRotation = virtualCamera.transform.localEulerAngles;
        currentRotation.y = rotationY;
        virtualCamera.transform.localRotation = Quaternion.Euler(currentRotation);
    }
}
