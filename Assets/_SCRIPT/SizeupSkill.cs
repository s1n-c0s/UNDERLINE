using UnityEngine;

public class SizeupSkill : MonoBehaviour
{
    private Vector3 _oldScale;
    [SerializeField] private Vector3 _newScale;

    [SerializeField] private int skillDurationTurns = 2; // Duration of the scale effect in turns
    private int remainingDurationTurns;

    [SerializeField] private int cooldownTurns = 3; // Number of turns for cooldown
    private int currentCooldownTurns;

    private bool skillActive;

    private void OnEnable()
    {
        PlayerController.OnPlayerStop += HandleTurnEnd;
        _oldScale = transform.localScale; // Store the original scale
        currentCooldownTurns = cooldownTurns; // Initialize the cooldown turns
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStop -= HandleTurnEnd;
    }

    private void HandleTurnEnd()
    {
        if (skillActive)
        {
            remainingDurationTurns--;
            if (remainingDurationTurns <= 0)
            {
                DeactivateEnemySkill();
                ResetCooldownTurns(); // Reset cooldown after skill deactivation
            }
        }

        DecreaseCooldownTurn(); // Decrease cooldown at the end of every turn
    }

    private void DecreaseCooldownTurn()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
            Debug.Log(gameObject.name + " Cooldown turns remaining: " + currentCooldownTurns);
            if (currentCooldownTurns == 0)
            {
                ActivateEnemySkill();
            }
        }
    }

    private void ActivateEnemySkill()
    {
        transform.localScale = _newScale;
        skillActive = true;
        remainingDurationTurns = skillDurationTurns;
    }

    private void DeactivateEnemySkill()
    {
        transform.localScale = _oldScale;
        skillActive = false;
    }

    private void ResetCooldownTurns()
    {
        currentCooldownTurns = cooldownTurns; // Reset to the initial number of turns
    }
}