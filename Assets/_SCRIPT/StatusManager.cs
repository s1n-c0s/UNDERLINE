using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public enum Status
    {
        Barrier,
        Burn,
        Panic,
        Heal
    }

    [Header("VFX")] 
    [SerializeField] private List<GameObject> fx_groups;

    private HealthSystem _healthSystem;
    private Coroutine burnCoroutine;
    private Coroutine healCoroutine;
    private EnemyDetectorArea enemyDetectorArea;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        enemyDetectorArea = FindObjectOfType<EnemyDetectorArea>();
        
        foreach (var _vfx in fx_groups)
        {
            if (_vfx != null)
            {
                _vfx.SetActive(false);
            }
        }

        // Check if the panic mode is already active and apply it
        if (enemyDetectorArea != null && enemyDetectorArea.IsInPanicMode() && !CompareTag("Player"))
        {
            ApplyStatus(Status.Panic, true);
        }
    }

    public void ApplyStatus(Status status, bool enable = true, int burnDamagePerSecond = 0, float burnDuration = 0, float healDuration = 0)
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
            case Status.Heal:
                if (enable)
                {
                    if (healCoroutine != null) StopCoroutine(healCoroutine);
                    healCoroutine = StartCoroutine(ApplyHealing(healDuration));
                }
                else
                {
                    if (healCoroutine != null) StopCoroutine(healCoroutine);
                    fx_groups[3].SetActive(false);
                }
                break;
        }
    }

    private void EnableBarrier(bool enable)
    {
        fx_groups[0].SetActive(enable);
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

    private IEnumerator ApplyHealing(float duration)
    {
        fx_groups[3].SetActive(true);
        yield return new WaitForSeconds(duration);
        fx_groups[3].SetActive(false);
    }
}
