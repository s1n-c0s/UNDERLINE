using System;
using UnityEngine;

public class SkillTurnSystem : MonoBehaviour
{
    public static event Action OnTurnEnd;

    public int SkillDurationTurns = 2;
    public int CooldownTurns = 3;

    [Header("CameraShake")]
    public bool useCamShake;
    public int camShakeTime = 2;
    public int camShakeDuration = 2;

    private int currentSkillCooldown;
    private int currentSkillDuration;

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

    public int GetCurrentValue()
    {
        return currentSkillDuration > 0 ? currentSkillDuration : currentSkillCooldown;
    }

    public void SetCurrentValue(int cooldown, int duration)
    {
        currentSkillCooldown = cooldown;
        currentSkillDuration = duration;
    }
}