using UnityEngine;

public class speedPad : MonoBehaviour
{
    [SerializeField] private float speedForce = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Combine the tag check to reduce redundancy
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Directly attempt to get and use the Rigidbody component
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                // Apply a force in the forward direction with the specified magnitude
                rb.AddForce(transform.forward * speedForce, ForceMode.Impulse);
            }
        }
    }
}