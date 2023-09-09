using System.Collections;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private float originalTimeScale;
    private float slowdownFactor = 0.5f; // Adjust this value to control the slowdown effect.
    private float slowdownDuration = 1.0f; // Adjust this value for the duration of slowdown.
    private bool isSlowed = false; // Keeps track of whether time is currently slowed down.

    void Start()
    {
        originalTimeScale = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSlowed)
            {
                // If time is already slowed, restore the original time scale.
                Time.timeScale = originalTimeScale;
                isSlowed = false;
            }
            else
            {
                // If time is not slowed, initiate the slowdown.
                StartCoroutine(SlowDownTime());
                isSlowed = true;
            }
        }
    }

    IEnumerator SlowDownTime()
    {
        Time.timeScale = slowdownFactor;

        // Wait for the specified duration.
        yield return new WaitForSeconds(slowdownDuration);

        // If you want time to return to normal automatically after the duration,
        // you can remove the following line and let it return to normal in Update.
        Time.timeScale = originalTimeScale;
        isSlowed = false;
    }
}