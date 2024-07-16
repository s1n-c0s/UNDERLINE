using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [SerializeField] private bool canFall = true;
    [SerializeField] private float Cooldown;
    [SerializeField] private GameObject _platform;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canFall)
        {
            _platform.SetActive(false);
            canFall = false;
            StartCoroutine(ResetPlatform());
        }
    }

    private IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(Cooldown);
        _platform.SetActive(true);
        canFall = true;
    }
}