using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnShake(float duration, float strength, int vibrato, float randomness)
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
        transform.DOShakeRotation(duration, strength, vibrato, randomness);
    }
    
    /*private void OnShake(float duration, float strength)
    {
        transform.DOShakePosition(duration, strength);
        transform.DOShakeRotation(duration, strength);
    }*/

    public static void Shake(float duration, float strength)
    {
        Instance.OnShake(duration, strength, 10, 90);
    }
}