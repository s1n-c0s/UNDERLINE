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

    private void Awake()
    {
        _skillSystem = GetComponent<SkillTurnSystem>();
        InitializeSkillSettings();
    }

    private void OnEnable()
    {
        _oldScale = transform.localScale;
        _skillSystem.updateSkill += HandleTurnEnd;
    }

    private void OnDisable()
    {
        _skillSystem.updateSkill -= HandleTurnEnd;
    }

    private void InitializeSkillSettings()
    {
        skillDurationTurns = _skillSystem.SkillDurationTurns;
        cooldownTurns = _skillSystem.CooldownTurns;
        camShakeTime = _skillSystem.camShakeTime;
        camShakeDuration = _skillSystem.camShakeDuration;
        ResetCooldown();
    }

    private void HandleTurnEnd()
    {
        switch (currentState)
        {
            case SkillState.Active:
                ProcessSkillDuration();
                break;
            case SkillState.Cooldown:
                ProcessCooldown();
                break;
            case SkillState.Idle:
                if (currentCooldownTurns == 0)
                {
                    ActivateSkill();
                }
                else
                {
                    DecrementCooldown();
                }
                break;
        }
    }

    private void ProcessSkillDuration()
    {
        remainingDurationTurns--;
        UpdateSkillSystemValues();

        if (remainingDurationTurns <= 0)
        {
            DeactivateSkill();
            StartCooldown();
        }
    }

    private void ProcessCooldown()
    {
        currentCooldownTurns--;
        UpdateSkillSystemValues();

        if (currentCooldownTurns <= 0)
        {
            currentState = SkillState.Idle;
            if (currentCooldownTurns == 0)
            {
                ActivateSkill();
            }
        }
    }

    private void DecrementCooldown()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
        }
        UpdateSkillSystemValues();

        if (currentCooldownTurns == 0 && currentState == SkillState.Idle)
        {
            ActivateSkill();
        }
    }

    private void ActivateSkill()
    {
        SpawnVFX();
        ShakeCamera();
        transform.localScale = _newScale;
        currentState = SkillState.Active;
        remainingDurationTurns = skillDurationTurns;
        ApplyExplosionForce();
        UpdateSkillSystemValues();
    }

    private void DeactivateSkill()
    {
        transform.localScale = _oldScale;
        currentState = SkillState.Idle;
        UpdateSkillSystemValues();
    }

    private void StartCooldown()
    {
        currentState = SkillState.Cooldown;
        currentCooldownTurns = cooldownTurns;
        UpdateSkillSystemValues();
    }

    private void ResetCooldown()
    {
        currentCooldownTurns = cooldownTurns;
        remainingDurationTurns = 0;
        UpdateSkillSystemValues();
    }

    private void UpdateSkillSystemValues()
    {
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
