using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject connectedPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;

        player.transform.position = connectedPortal.transform.position;

        UpdatePlayerRotation(player, connectedPortal.transform, playerVelocity);

        player.GetComponent<Rigidbody>().velocity = connectedPortal.transform.TransformVector(playerVelocity);
    }

    private void UpdatePlayerRotation(GameObject player, Transform destinationPortal, Vector3 playerVelocity)
    {
        Quaternion portalRotationDifference = destinationPortal.rotation * Quaternion.Inverse(transform.rotation);

        player.transform.rotation = portalRotationDifference * player.transform.rotation;

        Vector3 newForward = destinationPortal.TransformVector(playerVelocity.normalized);
        newForward.y = 0f;

        player.transform.forward = newForward;
    }
}