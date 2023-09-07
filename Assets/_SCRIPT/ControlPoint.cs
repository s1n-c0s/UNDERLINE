using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    private float xRot, yRot = 0f;
    private bool isDragging = false;

    public Rigidbody ball;
    public float rotationSpeed = 5f;
    public float shootPower = 30f;
    public LineRenderer line;

    private void OnMouseDrag()
    {
        isDragging = true;

        // Calculate rotation based on mouse movement.
        xRot += Input.GetAxis("Mouse X") * rotationSpeed;
        yRot += Input.GetAxis("Mouse Y") * rotationSpeed;

        // Limit the vertical rotation to prevent over-rotation.
        //yRot = Mathf.Clamp(yRot, -35f, 35f);

        // Apply the NEGATED rotation to the control point.
        transform.rotation = Quaternion.Euler(0f, -xRot, 0f);

        // Set the line's positions from control point to the shooting direction.
        line.gameObject.SetActive(true);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + transform.forward * 4f);
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            
            // Shoot the ball in the direction the control point is facing.
            ball.velocity = transform.forward * shootPower;
            line.gameObject.SetActive(false);
        }
    }
}