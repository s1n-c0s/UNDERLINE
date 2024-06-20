using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public enum Status
    {
        Barrier,
        Burn
    }

    [Header("VFX")]
    [SerializeField] private GameObject fx_Barrier;
    [SerializeField] private GameObject fx_Fire;

    private HealthSystem _healthSystem;
    private Coroutine burnCoroutine;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        if (fx_Barrier != null) 
        {
            fx_Barrier.SetActive(false);
        }
        fx_Fire.SetActive(false);
    }

    public void ApplyStatus(Status status, bool enable, int burnDamagePerSecond = 0, float burnDuration = 0)
    {
        switch (status)
        {
            case Status.Barrier:
                EnableBarrier(enable);
                break;
            case Status.Burn:
                if (enable)
                {
                    if (burnCoroutine != null) StopCoroutine(burnCoroutine);
                    burnCoroutine = StartCoroutine(ApplyBurnDamage(burnDamagePerSecond, burnDuration));
                }
                else
                {
                    if (burnCoroutine != null) StopCoroutine(burnCoroutine);
                    fx_Fire.SetActive(false);
                }
                break;
        }
    }

    private void EnableBarrier(bool enable)
    {
        fx_Barrier.gameObject.SetActive(enable);
        _healthSystem.SetProtection(enable);
    }

    private IEnumerator ApplyBurnDamage(int damage, float duration)
    {
        fx_Fire.SetActive(true);
        while (duration > 0)
        {
            _healthSystem.TakeDamage(damage);
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        fx_Fire.SetActive(false);
    }
}