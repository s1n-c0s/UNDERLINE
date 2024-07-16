using System.Collections;
using UnityEngine;

public class SlowPlatform : MonoBehaviour
{
    public AnimationCurve speedCurve;
    public float slowDuration = 2.0f; // Duration over which the speed will decrease
    public float slowMultiplier = 0.5f; // Multiplier for speed reduction
    private bool isSlowing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSlowing = true;
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            StartCoroutine(SlowDown(playerRigidbody));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSlowing = false;
        }
    }

    private IEnumerator SlowDown(Rigidbody playerRigidbody)
    {
        float initialSpeed = playerRigidbody.velocity.magnitude;
        float elapsedTime = 0f;

        while (isSlowing)
        {
            float t = Mathf.Clamp01(elapsedTime / slowDuration);
            float speedMultiplier = Mathf.Lerp(1f, slowMultiplier, speedCurve.Evaluate(t));
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * initialSpeed * speedMultiplier;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}