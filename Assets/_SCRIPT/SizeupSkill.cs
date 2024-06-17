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
    [SerializeField] private float explosionForce = 1500f; // Increased explosion force
    [SerializeField] private int camShakeTime = 2;
    [SerializeField] private int camShakeDuration = 2;

    private int skillDurationTurns;
    private int cooldownTurns;

    private int remainingDurationTurns;
    private int currentCooldownTurns;

    private enum SkillState { Idle, Active, Cooldown }
    private SkillState currentState = SkillState.Idle;

    private void OnEnable()
    {
        SkillSystem.OnTurnEnd += HandleTurnEnd;
        _oldScale = transform.localScale; // Store the original scale

        SkillSystem _skillSystem = FindObjectOfType<SkillSystem>();
      
    }

    private void OnDisable()
    {
        SkillSystem.OnTurnEnd -= HandleTurnEnd;

        SkillSystem _skillSystem = FindObjectOfType<SkillSystem>();
      
    }

    public void SetTurnSettings(int duration, int cooldown)
    {
        skillDurationTurns = duration;
        cooldownTurns = cooldown;
        ResetCooldownTurns();
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
                DecreaseCooldownTurn();
                break;
        }
    }

    private void HandleSkillDuration()
    {
        remainingDurationTurns--;
        if (remainingDurationTurns <= 0)
        {
            DeactivateEnemySkill();
            StartCooldown();
        }
    }

    private void HandleCooldown()
    {
        currentCooldownTurns--;
        if (currentCooldownTurns <= 0)
        {
            currentState = SkillState.Idle;
            DecreaseCooldownTurn();
        }
    }

    private void DecreaseCooldownTurn()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
        }

        if (currentCooldownTurns == 0 && currentState == SkillState.Idle)
        {
            ActivateEnemySkill();
        }
    }

    public void ActivateEnemySkill()
    {
        // Spawn VFX
        GameObject vfx = LeanPool.Spawn(fx_cloud, transform);
        LeanPool.Despawn(vfx, 5f);

        CameraShake.Shake(camShakeTime, camShakeDuration);

        // Change scale
        transform.localScale = _newScale;
        currentState = SkillState.Active;
        remainingDurationTurns = skillDurationTurns;

        // Apply explosion force to nearby objects, but not to itself
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

    private void DeactivateEnemySkill()
    {
        transform.localScale = _oldScale;
        currentState = SkillState.Idle;
    }

    private void StartCooldown()
    {
        currentState = SkillState.Cooldown;
        currentCooldownTurns = cooldownTurns;
    }

    private void ResetCooldownTurns()
    {
        currentCooldownTurns = cooldownTurns; // Reset to the initial number of turns
    }

    // Draw Gizmos to visualize the explosion radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
