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
    private bool inCooldown;

    private void OnEnable()
    {
        PlayerController.OnPlayerStop += HandleTurnEnd;
        _oldScale = transform.localScale; // Store the original scale
        remainingDurationTurns = 0; // Initialize to 0
        currentCooldownTurns = cooldownTurns; // Initialize to cooldownTurns
        skillActive = false;
        inCooldown = false;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStop -= HandleTurnEnd;
    }

    private void HandleTurnEnd()
    {
        if (skillActive)
        {
            HandleSkillDuration();
        }
        else if (inCooldown)
        {
            HandleCooldown();
        }
        else
        {
            DecreaseCooldownTurn(); // Decrease cooldown at the end of every turn if not in cooldown or skill active
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
            inCooldown = false;
            DecreaseCooldownTurn(); // Restart the cooldown countdown
        }
    }

    private void DecreaseCooldownTurn()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
            Debug.Log(gameObject.name + " Cooldown turns remaining: " + currentCooldownTurns);
        }
        
        if (currentCooldownTurns == 0 && !skillActive && !inCooldown)
        {
            ActivateEnemySkill();
        }
    }

    private void StartCooldown()
    {
        inCooldown = true;
        currentCooldownTurns = cooldownTurns;
    }

    public void ActivateEnemySkill()
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
}
