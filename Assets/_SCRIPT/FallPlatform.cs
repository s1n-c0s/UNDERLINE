using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FallPlatform : MonoBehaviour
{
    [SerializeField] private bool canFall = true;
    [SerializeField] private float Countdown = 2f; // Time before the platform falls
    [SerializeField] private float Cooldown = 5f; // Time before the platform resets
    [SerializeField] private GameObject _platform;
    [SerializeField] private Collider hitbox;
    
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 1f; // Duration of the shake
    [SerializeField] private float shakeStrength = 1f; // Strength of the shake
    [SerializeField] private int shakeVibrato = 10; // Vibrato of the shake
    [SerializeField] private float shakeRandomness = 90f; // Randomness of the shake

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) && canFall)
        {
            StartCoroutine(CountdownToFall());
        }
    }

    private IEnumerator CountdownToFall()
    {
        // Start the shake effect
        _platform.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
        
        yield return new WaitForSeconds(Countdown);
        _platform.SetActive(false);
        hitbox.enabled = false;
        canFall = false;
        StartCoroutine(ResetPlatform());
    }

    private IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(Cooldown);
        _platform.SetActive(true);
        hitbox.enabled = true;
        canFall = true;
    }
}
