using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPad : MonoBehaviour
{

    public float jumpForce = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player or has a specific tag.
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Apply a force to the player's Rigidbody to make them jump.
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}