using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public static event Action OnTurnEnd;

    // Default settings for skills
    [SerializeField] private int SkillDurationTurns = 2; // Duration of the scale effect in turns
    [SerializeField] private int CooldownTurns = 3; // Number of turns for cooldown

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
}
