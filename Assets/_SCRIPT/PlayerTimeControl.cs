using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerTimeControl : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f; // Adjust this value based on your game's needs.
    public LayerMask enemyLayer; // The layer where your enemies are located.

    [Header("Time Control Settings")]
    public float slowdownFactor = 0.5f; // Adjust the time slowdown factor.
    public float slowdownDuration = 0.5f; // Adjust the duration of the slowdown effect.

    [Header("Volume Control Settings")]
    public GameObject volumeControlObject; // Assign the GameObject with the Volume component in the Inspector.
    public VolumeProfile slowedTimeProfile; // Assign this in the Inspector.

    private Volume volumeComponent;
    private VolumeProfile defaultProfile;
    private bool isDetected = false;

    private void Start()
    {
        InitializeReferences();
    }

    private void Update()
    {
        if (DetectEnemies())
        {
            if (!isDetected)
            {
                ApplySlowdownEffect();
                isDetected = true;
            }
        }
        else if (isDetected)
        {
            ResetEffect();
            isDetected = false;
        }
    }

    private void InitializeReferences()
    {
        volumeComponent = volumeControlObject.GetComponent<Volume>();
        defaultProfile = volumeComponent.profile;
    }

    private bool DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        return colliders.Length > 0;
    }

    private void ApplySlowdownEffect()
    {
        SetTimeScale(slowdownFactor);
        SetVolumeProfile(slowedTimeProfile);

        StartCoroutine(ResetAfterDuration());
    }

    private void ResetEffect()
    {
        SetTimeScale(1f);
        SetVolumeProfile(defaultProfile);
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    private void SetVolumeProfile(VolumeProfile profile)
    {
        volumeComponent.profile = profile;
    }

    private IEnumerator ResetAfterDuration()
    {
        yield return new WaitForSeconds(slowdownDuration);
        ResetEffect();
    }

    // Draw the detection radius gizmo in the Unity editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
