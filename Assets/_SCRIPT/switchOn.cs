using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOn : MonoBehaviour
{
    public GameObject platform; // Assign the object you want to move
    public Transform targetPosition; // Assign the target position or empty game object
    public float moveSpeed = 2f; // Speed of movement

    private bool switched = false;
    private Vector3 initialPosition;
    private float journeyLength;

    private void Start()
    {
        initialPosition = platform.transform.position;
        if (targetPosition != null)
        {
            journeyLength = Vector3.Distance(initialPosition, targetPosition.position);
        }
        else
        {
            Debug.LogError("Target position is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!switched && other.CompareTag("Player")) // Check if obj1 collided and the switch hasn't been triggered yet
        {
            switched = true;
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float distanceCovered = 0f;
        float startTime = Time.time;

        while (distanceCovered < journeyLength)
        {
            float fractionOfJourney = (Time.time - startTime) * moveSpeed / journeyLength;
            platform.transform.position = Vector3.Lerp(initialPosition, targetPosition.position, fractionOfJourney);
            distanceCovered = Vector3.Distance(platform.transform.position, initialPosition);
            yield return null;
        }
    }
}
