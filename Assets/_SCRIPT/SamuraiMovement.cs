using UnityEngine;

public class SamuraiMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float movementSpeed = 5.0f;
    public float stopDuration = 3.0f;

    private int currentWaypointIndex = 0;
    private float timeSinceLastStop = 0.0f;
    private bool isMovingForward = true;

    private void Update()
    {
        // Check if there are waypoints defined
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy.");
            return;
        }

        // Calculate direction to the current waypoint
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // Move the enemy
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        // Check if the enemy has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Stop for the specified duration
            if (timeSinceLastStop < stopDuration)
            {
                timeSinceLastStop += Time.deltaTime;
            }
            else
            {
                timeSinceLastStop = 0.0f;
                NextWaypoint();
            }
        }
    }

    private void NextWaypoint()
    {
        if (isMovingForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 1;
                isMovingForward = false;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 0;
                isMovingForward = true;
            }
        }
    }
}
