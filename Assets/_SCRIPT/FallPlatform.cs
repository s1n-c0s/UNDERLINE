using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [SerializeField] private bool canFall = true;
    [SerializeField] private float Countdown = 2f; // Time before the platform falls
    [SerializeField] private float Cooldown = 5f; // Time before the platform resets
    [SerializeField] private GameObject _platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canFall)
        {
            StartCoroutine(CountdownToFall());
        }
    }

    private IEnumerator CountdownToFall()
    {
        yield return new WaitForSeconds(Countdown);
        _platform.SetActive(false);
        canFall = false;
        StartCoroutine(ResetPlatform());
    }

    private IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(Cooldown);
        _platform.SetActive(true);
        canFall = true;
    }
}