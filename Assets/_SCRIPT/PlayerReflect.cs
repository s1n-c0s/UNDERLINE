using UnityEngine;

public class PlayerReflect : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        ReflectVelocity(other.contacts[0].normal);
    }

    private void ReflectVelocity(Vector3 normal)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, normal);

        // Only change the forward direction without affecting the up direction
        direction.y = transform.forward.y;

        rb.velocity = direction * Mathf.Max(speed, 0f);

        // Make the player face in the reflection direction
        transform.LookAt(transform.position + direction);
    }
}