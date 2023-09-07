using UnityEngine;

public class DragBackAndShoot : MonoBehaviour
{
    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;
    private Vector2 dragDirection;
    private bool isDragging = false;

    public float shootForce = 10.0f; // Adjust this value to control the shooting force.
    public float maxDragDistance = 3.0f; // Adjust this value to control the maximum drag distance.

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Record the start position of the drag.
            dragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            // Update the end position of the drag.
            dragEndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragDirection = (dragEndPosition - dragStartPosition).normalized;

            // Calculate the backward drag distance (opposite to the direction).
            float backwardDragDistance = Mathf.Clamp(Vector2.Distance(dragStartPosition, dragEndPosition), 0, maxDragDistance);

            // Visualize the backward drag distance (you can remove this for the final version).
            Debug.DrawLine(transform.position, transform.position - new Vector3(dragDirection.x, dragDirection.y, 0) * backwardDragDistance, Color.red);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            // Apply a force to the Rigidbody in the opposite direction of the drag.
            Vector3 shootDirection = -new Vector3(dragDirection.x, dragDirection.y, 0);
            rb.velocity = Vector3.zero; // Reset velocity before applying force.
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

            isDragging = false;
        }
    }
}