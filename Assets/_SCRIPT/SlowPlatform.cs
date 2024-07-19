using System.Collections;
using UnityEngine;

public class SlowPlatform : MonoBehaviour
{
    public AnimationCurve speedCurve;
    public float slowDuration = 2.0f; // Duration over which the speed will decrease
    public float slowMultiplier = 0.5f; // Multiplier for speed reduction

    private bool isSlowing = false;
    private Coroutine slowCoroutine;
    private Rigidbody playerRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isSlowing)
            {
                isSlowing = true;
                playerRigidbody = other.GetComponent<Rigidbody>();
                slowCoroutine = StartCoroutine(SlowDown());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSlowing)
            {
                isSlowing = false;
                if (slowCoroutine != null)
                {
                    StopCoroutine(slowCoroutine);
                    slowCoroutine = null;
                }
            }
        }
    }

    private IEnumerator SlowDown()
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

        // Ensure the velocity is restored if the player exits the platform
        playerRigidbody.velocity = playerRigidbody.velocity.normalized * initialSpeed;
    }
}