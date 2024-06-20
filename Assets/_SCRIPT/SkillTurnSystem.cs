using System;
using UnityEngine;

public class SkillTurnSystem : MonoBehaviour
{
    public int SkillDurationTurns = 2;
    public int CooldownTurns = 3;

    [Header("CameraShake")]
    public bool useCamShake;
    public int camShakeTime = 2;
    public int camShakeDuration = 2;

    private int currentSkillCooldown;
    private int currentSkillDuration;

    public int CurrentCooldownTurns => currentSkillCooldown;
    public int CurrentDurationTurns => currentSkillDuration;

    public event Action OnTurnEnd;

    private void OnEnable()
    {
        PlayerController.OnPlayerStop += HandleTurnEnd;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerStop -= HandleTurnEnd;
    }

    private void HandleTurnEnd()
    {
        OnTurnEnd?.Invoke();
    }

    public void SetCurrentValue(int cooldown, int duration)
    {
        currentSkillCooldown = cooldown;
        currentSkillDuration = duration;
    }

    public int GetCurrentValue()
    {
        return currentSkillDuration > 0 ? currentSkillDuration : currentSkillCooldown;
    }
}