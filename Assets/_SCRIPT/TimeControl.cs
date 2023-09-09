using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeControl : MonoBehaviour
{
    private float originalTimeScale;
    private float slowdownFactor = 0.5f;
    private float slowdownDuration = 1.0f;
    private bool isSlowed = false;

    private Volume volume; // Reference to the URP Volume component.
    private VolumeProfile normalProfile; // The original profile.
    public VolumeProfile slowedProfile; // Assign the slowed down profile in the Inspector.

    void Start()
    {
        originalTimeScale = Time.timeScale;
        volume = GetComponent<Volume>();
        normalProfile = volume.profile;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSlowed)
            {
                // If time is already slowed, restore the original time scale and profile.
                Time.timeScale = originalTimeScale;
                volume.profile = normalProfile;
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

        // Apply the slowed profile when time is slowed down.
        volume.profile = slowedProfile;

        // Wait for the specified duration.
        yield return new WaitForSeconds(slowdownDuration);

        // If you want time to return to normal automatically after the duration,
        // you can remove the following line and let it return to normal in Update.
        Time.timeScale = originalTimeScale;
        volume.profile = normalProfile;
        isSlowed = false;
    }
}