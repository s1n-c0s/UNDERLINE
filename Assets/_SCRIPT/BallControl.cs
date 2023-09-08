using UnityEngine;

public class BallControl : MonoBehaviour
{
    public float maxDragDistance = 5f; // Maximum distance the player can drag the ball
    public float maxForce = 500f; // Maximum force applied to the ball
    public LineRenderer shotLineRenderer; // Reference to the Line Renderer component
    private Rigidbody rb;
    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private Vector3 shootDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotLineRenderer.enabled = false; // Disable the Line Renderer initially
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Record the start position of the drag
            dragStartPos = GetMouseWorldPosition();
            shotLineRenderer.enabled = true; // Enable the Line Renderer when dragging begins
        }

        if (Input.GetMouseButton(0))
        {
            // Record the end position of the drag
            dragEndPos = GetMouseWorldPosition();

            // Calculate the shoot direction based on the drag
            shootDirection = (dragStartPos - dragEndPos).normalized;

            // Ensure the drag distance does not exceed the maximum allowed
            float dragDistance = Vector3.Distance(dragStartPos, dragEndPos);
            if (dragDistance > maxDragDistance)
            {
                dragEndPos = dragStartPos - (shootDirection * maxDragDistance);
            }

            // Update the Line Renderer to visualize the shot direction
            shotLineRenderer.SetPosition(0, transform.position);
            shotLineRenderer.SetPosition(1, dragEndPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Apply force to the ball when the mouse button is released
            float forceMagnitude = Mathf.Clamp(maxDragDistance - Vector3.Distance(dragStartPos, dragEndPos), 0, maxDragDistance) * maxForce;
            rb.AddForce(shootDirection * forceMagnitude);
            shotLineRenderer.enabled = false; // Disable the Line Renderer after shooting
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Adjust the Z value to your scene's depth
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
