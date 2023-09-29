using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReflect : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0f);

        // Make the player face in the reflection direction
        transform.LookAt(transform.position + direction);
    }
}