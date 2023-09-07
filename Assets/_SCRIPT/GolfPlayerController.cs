using UnityEngine;

public class GolfPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;
    private bool isDragging = false;
    private float maxDragDistance = 5.0f;
    private float maxHitForce = 1000.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPosition = GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            dragEndPosition = GetMouseWorldPosition();

            // Calculate the force based on the drag distance
            float dragDistance = Vector3.Distance(dragStartPosition, dragEndPosition);
            float hitForce = Mathf.Clamp(dragDistance * 10.0f, 0, maxHitForce);

            // Calculate the direction of the shot
            Vector3 shootDirection = (dragStartPosition - dragEndPosition).normalized;

            // Apply the force to the ball
            rb.AddForce(shootDirection * hitForce);

            // Reset the ball's position
            ResetBallPosition();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void ResetBallPosition()
    {
        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
