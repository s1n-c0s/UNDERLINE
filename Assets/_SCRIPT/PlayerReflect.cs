using UnityEngine;

public class PlayerReflect : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Record the velocity in FixedUpdate for accurate physics calculations
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            ReflectVelocity(other.contacts[0].normal);
        }

    }

    private void ReflectVelocity(Vector3 normal)
    {
        // Calculate the reflection direction while preserving speed
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, normal);

        // Only change the forward direction without affecting the up direction
        direction = Vector3.ProjectOnPlane(direction, transform.up).normalized;

        rb.velocity = direction * speed;

        // Make the player face in the reflection direction
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}
