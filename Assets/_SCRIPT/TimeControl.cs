using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeControl : MonoBehaviour
{
    private float originalTimeScale;
    private float slowdownFactor = 0.2f; // Adjust this value to control the slowdown effect.
    private float slowdownDuration = 1.0f; // Adjust this value for the duration of slowdown.
    private bool isSlowed = false; // Tracks whether time is currently slowed down.

    private Volume volume; // Reference to the URP Volume component.
    private VolumeProfile normalProfile; // The original profile.
    public VolumeProfile slowedProfile; // Assign the slowed down profile in the Inspector.

    private void Start()
    {
        // Initialize originalTimeScale and get references to the Volume components.
        originalTimeScale = Time.timeScale;
        volume = GetComponent<Volume>();
        normalProfile = volume.profile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleSlowMotion(); // Check for spacebar input to toggle slow motion.
        }
    }

    private void ToggleSlowMotion()
    {
        if (isSlowed)
        {
            ResumeNormalTime(); // If time is already slowed, restore normal time.
        }
        else
        {
            SlowDownTime(); // If time is not slowed, initiate slow motion.
        }
    }

    private void SlowDownTime()
    {
        // Set time scale to slow motion, apply the slowed profile, and start a coroutine to return to normal time.
        Time.timeScale = slowdownFactor;
        volume.profile = slowedProfile;
        isSlowed = true;
        StartCoroutine(WaitAndResumeTime());
    }

    private void ResumeNormalTime()
    {
        // Restore normal time scale and the normal profile.
        Time.timeScale = originalTimeScale;
        volume.profile = normalProfile;
        isSlowed = false;
    }

    private IEnumerator WaitAndResumeTime()
    {
        // Wait for the specified duration and then resume normal time.
        yield return new WaitForSeconds(slowdownDuration);
        ResumeNormalTime();
    }
}
