using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destinationObject; // GameObject ของประตูปลายทาง
    private bool isTeleporting;

    private void OnEnable()
    {
        // Ensure that isTeleporting is reset when the script is re-enabled
        isTeleporting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            TeleportPlayer(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleporting = false;
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (destinationObject == null)
        {
            Debug.LogError("Destination portal not set for " + gameObject.name);
            return;
        }

        // Get the position and forward direction of the destination object
        Vector3 destinationPosition = destinationObject.transform.position;
        Vector3 destinationForward = destinationObject.transform.forward;

        // Preserve the player's velocity
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        Vector3 originalVelocity = playerRigidbody.velocity;

        // Teleport the player to the destination position
        player.transform.position = destinationPosition;

        // Calculate the rotation to align the player with the portal's forward direction
        Quaternion rotationToAlign = Quaternion.FromToRotation(player.transform.forward, destinationForward);

        // Apply the rotation to the player
        player.transform.rotation = rotationToAlign * player.transform.rotation;

        // Adjust the velocity direction without changing its magnitude
        playerRigidbody.velocity = rotationToAlign * originalVelocity;

        // Allow the player to teleport again after a short delay
        Invoke("ResetTeleportFlag", 0.5f);
    }

    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }
}
