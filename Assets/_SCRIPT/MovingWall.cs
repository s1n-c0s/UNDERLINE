using System.Collections;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] private Transform[] points; // Array of points for the platform to move between
    [SerializeField] private float speed = 2.0f; // Speed of the platform's movement
    [SerializeField] private float waitTime = 1.0f; // Time to wait at each point before moving to the next one

    private Transform platformTransform;
    private int currentPointIndex = 0; // Index of the current target point

    private void Start()
    {
        platformTransform = transform;

        // Ensure there's at least one point in the array to move to
        if (points.Length > 0)
        {
            // Move the platform to the start point immediately without waiting
            platformTransform.position = points[currentPointIndex].position;
            // Start the moving process
            StartCoroutine(MovePlatform());
        }
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            // Move towards the current target point
            Transform targetPoint = points[currentPointIndex];
            while (Vector3.Distance(platformTransform.position, targetPoint.position) > 0.001f)
            {
                platformTransform.position = Vector3.MoveTowards(platformTransform.position, targetPoint.position, speed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            // Wait at the current point
            yield return new WaitForSeconds(waitTime);

            // Update the index to target the next point, looping back if at the end of the array
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }

    // Optional: Visualize the path of the platform in the editor
    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 2)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < points.Length; i++)
        {
            // Draw a line to the next point, looping back to the first point from the last one
            Vector3 startPoint = points[i].position;
            Vector3 endPoint = points[(i + 1) % points.Length].position;
            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}
