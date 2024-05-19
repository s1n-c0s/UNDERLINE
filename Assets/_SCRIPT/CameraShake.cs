using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void OnShake(float duration, float strength, int vibrato, float randomness)
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        transform.DOComplete(); // Complete any ongoing animations on this transform

        transform.DOShakePosition(duration, strength, vibrato, randomness);
        transform.DOShakeRotation(duration, strength, vibrato, randomness)
            .OnComplete(() =>
            {
                transform.position = originalPosition; // Reset position to original
                transform.rotation = originalRotation; // Reset rotation to original
            });
    }
    

    public static void Shake(float duration, float strength)
    {
        Instance.OnShake(duration, strength, 10, 90);
    }
}