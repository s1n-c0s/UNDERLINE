using System;
using Lean.Pool;
using UnityEngine;

public class SizeupSkill : MonoBehaviour
{
    private Vector3 _oldScale;
    [SerializeField] private Vector3 _newScale;

    [Header("VFX")]
    [SerializeField] private GameObject fx_cloud;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 1500f;

    private int skillDurationTurns;
    private int cooldownTurns;
    private int camShakeTime;
    private int camShakeDuration;

    private int remainingDurationTurns;
    private int currentCooldownTurns;

    private enum SkillState { Idle, Active, Cooldown }
    private SkillState currentState = SkillState.Idle;

    [SerializeField] private SkillTurnSystem _skillSystem;

    private void Start()
    {
        _skillSystem = GetComponent<SkillTurnSystem>();
        SetTurnSettings(_skillSystem.useCamShake);
    }

    private void OnEnable()
    {
        _oldScale = transform.localScale;
        SkillTurnSystem.OnTurnEnd += HandleTurnEnd;
    }

    private void OnDisable()
    {
        SkillTurnSystem.OnTurnEnd -= HandleTurnEnd;
    }

    public void SetTurnSettings(bool useCamShake)
    {
        skillDurationTurns = _skillSystem.SkillDurationTurns;
        cooldownTurns = _skillSystem.CooldownTurns;
        ResetCooldownTurns();
        if (useCamShake)
        {
            camShakeTime = _skillSystem.camShakeTime;
            camShakeDuration = _skillSystem.camShakeDuration;
        }
    }

    private void HandleTurnEnd()
    {
        switch (currentState)
        {
            case SkillState.Active:
                HandleSkillDuration();
                break;
            case SkillState.Cooldown:
                HandleCooldown();
                break;
            case SkillState.Idle:
                if (currentCooldownTurns == 0)
                {
                    ActivateEnemySkill();
                }
                else
                {
                    DecreaseCooldownTurn();
                }
                break;
        }
    }

    private void HandleSkillDuration()
    {
        remainingDurationTurns--;
        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);

        if (remainingDurationTurns <= 0)
        {
            DeactivateEnemySkill();
            StartCooldown();
        }
    }

    private void HandleCooldown()
    {
        currentCooldownTurns--;
        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);

        if (currentCooldownTurns <= 0)
        {
            currentState = SkillState.Idle;
            // Ensure the skill is activated immediately when the cooldown ends
            if (currentCooldownTurns == 0)
            {
                ActivateEnemySkill();
            }
        }
    }

    private void DecreaseCooldownTurn()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
        }

        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);

        // Ensure the skill is activated immediately when the cooldown ends
        if (currentCooldownTurns == 0 && currentState == SkillState.Idle)
        {
            ActivateEnemySkill();
        }
    }

    public void ActivateEnemySkill()
    {
        SpawnVFX();
        ShakeCamera();

        transform.localScale = _newScale;
        currentState = SkillState.Active;
        remainingDurationTurns = skillDurationTurns;

        ApplyExplosionForce();

        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);
    }

    private void DeactivateEnemySkill()
    {
        transform.localScale = _oldScale;
        currentState = SkillState.Idle;

        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);
    }

    private void StartCooldown()
    {
        currentState = SkillState.Cooldown;
        currentCooldownTurns = cooldownTurns;
        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);
    }

    private void ResetCooldownTurns()
    {
        currentCooldownTurns = cooldownTurns;
        remainingDurationTurns = 0;
        _skillSystem.SetCurrentValue(currentCooldownTurns, remainingDurationTurns);
    }

    private void SpawnVFX()
    {
        GameObject vfx = LeanPool.Spawn(fx_cloud, transform);
        LeanPool.Despawn(vfx, 5f);
    }

    private void ShakeCamera()
    {
        if (_skillSystem.useCamShake)
        {
            CameraShake.Shake(camShakeTime, camShakeDuration);
        }
    }

    private void ApplyExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject != gameObject)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
