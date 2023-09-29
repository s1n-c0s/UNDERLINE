using System;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float impulseForce = 10.0f; // Public variable to control the impulse force
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            rb.AddForce((other.transform.position - transform.position) * impulseForce * rb.mass, ForceMode.Impulse);
        }
    }
}