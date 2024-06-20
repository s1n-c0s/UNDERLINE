using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public enum Status
    {
        Barrier,
        Burn,
        Panic
    }

    [Header("VFX")] [SerializeField] private List<GameObject> fx_groups;

    private HealthSystem _healthSystem;
    private Coroutine burnCoroutine;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        foreach (var _vfx in fx_groups)
        {
            if (_vfx != null)
            {
                _vfx.SetActive(false);
            }
        }
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
                    fx_groups[1].SetActive(false);
                }
                break;
            case Status.Panic:
                isPanic(enable);
                break;
        }
    }

    private void EnableBarrier(bool enable)
    {
        fx_groups[0].gameObject.SetActive(enable);
        _healthSystem.SetProtection(enable);
    }

    private IEnumerator ApplyBurnDamage(int damage, float duration)
    {
        fx_groups[1].SetActive(true);
        while (duration > 0)
        {
            _healthSystem.TakeDamage(damage);
            yield return new WaitForSeconds(1f);
            duration -= 1f;
        }
        fx_groups[1].SetActive(false);
    }

    private void isPanic(bool enable)
    {
        fx_groups[2].SetActive(enable);
    }
}