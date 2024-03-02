using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raftMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float stopDuration = 5.0f;

    // Define the vertices of the rectangle
    [SerializeField] private List<Vector3> squareVertices = new List<Vector3>();

    private Vector3[] path; // The path to follow
    private int currentTargetIndex = 0; // Current index in the path array
    private bool isMoving = true;


    public void Start()
    {
        if (squareVertices.Count != 4)
        {
            Debug.LogError("Please define exactly 4 vertices for the square.");
            return;
        }

        // Set the initial path to follow
        path = squareVertices.ToArray();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move towards the current target
            transform.position = Vector3.MoveTowards(transform.position, path[currentTargetIndex], moveSpeed * Time.deltaTime);

            // Check if reached the current target
            if (transform.position == path[currentTargetIndex])
            {
                // Move to the next target
                currentTargetIndex = (currentTargetIndex + 1) % path.Length;

                // Stop moving and rotate after reaching each corner
                isMoving = false;
                StartCoroutine(WaitAndRotate());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndRotate()
    {
        // Wait for stopDuration seconds
        yield return new WaitForSeconds(stopDuration);

        // Rotate 90 degrees when moving to the next corner
        //transform.Rotate(Vector3.up, 90.0f);

        // Resume moving
        isMoving = true;

        // Close the Katana when starting to move again

    }
}