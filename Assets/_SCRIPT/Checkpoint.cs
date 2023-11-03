using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Store the last position
    private Vector3 lastPosition;

    // Call this function to set the checkpoint
    public void SetCheckpointPosition()
    {
        // Set the current position as the last position
        lastPosition = transform.position;
    }

    // Call this function to retrieve the last checkpoint position
    public Vector3 GetLastCheckpointPosition()
    {
        return lastPosition;
    }
}