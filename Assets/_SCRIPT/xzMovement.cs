using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xzMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float stopDuration = 5.0f;

    [SerializeField] private Vector3 startPoint = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 endPoint = new Vector3(10.0f, 0.0f, 10.0f);

    private Vector3 target;
    private bool isMoving = true;


    private void Start()
    {
        target = endPoint; // Start from the end point
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the Samurai towards the target (Modified to move along the x and z axes)
            Vector3 newPosition = new Vector3(target.x, transform.position.y, target.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

            // When the Samurai reaches the target, stop
            if (transform.position == newPosition)
            {
                // Stop moving for stopDuration seconds
                isMoving = false;

                StartCoroutine(WaitAndRotate());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndRotate()
    {
        // Wait for stopDuration seconds
        yield return new WaitForSeconds(stopDuration);

        // Change target
        if (target == startPoint)
            target = endPoint;
        else
            target = startPoint;

        // Rotate towards the new target
        transform.rotation = Quaternion.LookRotation(target - transform.position);

        // Reset to start a new movement
        isMoving = true;


    }

}