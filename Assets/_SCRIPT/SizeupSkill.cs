using Lean.Pool;
using UnityEngine;

public class SizeupSkill : MonoBehaviour
{
    private Vector3 _oldScale;
    [SerializeField] private Vector3 _newScale;

    [SerializeField] private int skillDurationTurns = 2; // Duration of the scale effect in turns
    [SerializeField] private int cooldownTurns = 3; // Number of turns for cooldown

    [Header("VFX")] [SerializeField] private GameObject fx_cloud; 
    private int remainingDurationTurns;
    private int currentCooldownTurns;

    private enum SkillState { Idle, Active, Cooldown }
    private SkillState currentState = SkillState.Idle;

    private void OnEnable()
    {
        PlayerController.OnPlayerStop += HandleTurnEnd;
        _oldScale = transform.localScale; // Store the original scale
        ResetCooldownTurns();
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStop -= HandleTurnEnd;
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
            /*Debug.Log($"{gameObject.name} Cooldown turns remaining: {currentCooldownTurns}");*/
        }

        if (currentCooldownTurns == 0 && currentState == SkillState.Idle)
        {
            ActivateEnemySkill();
        }
    }

    public void ActivateEnemySkill()
    {
        GameObject vfx = LeanPool.Spawn(fx_cloud, transform);
        LeanPool.Despawn(vfx, 5f);
        transform.localScale = _newScale;
        currentState = SkillState.Active;
        remainingDurationTurns = skillDurationTurns;
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
}
