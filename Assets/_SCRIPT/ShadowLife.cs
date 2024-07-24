using System;
using System.Collections.Generic;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

public class ShadowLife : MonoBehaviour
{
    [SerializeField] private List<GameObject> shadowModels;
    [SerializeField] private ParticleSystem shadowDieFx;
    [SerializeField] private int cooldownToResetHealth = 2;

    [Header("VFX")]
    [SerializeField] private ParticleSystem fx_switch;
    
    private bool isDuringCooldown;
    private int currentCooldown;
    private HealthSystem healthSystem;
    private ParticleSystem activeShadowDieFx;

    private void Start()
    {
        isDuringCooldown = false;
        currentCooldown = cooldownToResetHealth;
        healthSystem = GetComponent<HealthSystem>();
        shadowDieFx = healthSystem.fx_die;
    }

    private void OnEnable()
    {
        PlayerController.OnTurnEnd += HandleTurnEnd;
    }

    private void OnDisable()
    {
        PlayerController.OnTurnEnd -= HandleTurnEnd;
    }

    private void HandleTurnEnd()
    {
        if (isDuringCooldown)
        {
            HandleCooldown();
        }
        else
        {
            HandleNormalTurn();
        }
    }

    private void HandleCooldown()
    {
        currentCooldown--;

        if (currentCooldown <= 0)
        {
            ResetHealthAndCooldown();
        }
    }

    private void HandleNormalTurn()
    {
        if (healthSystem.GetCurrentHealth() <= 0)
        {
            TriggerShadowDeath();
        }
    }

    private void TriggerShadowDeath()
    {
        gameObject.tag = "Untagged";
        activeShadowDieFx = LeanPool.Spawn(shadowDieFx, transform.position, Quaternion.identity);
        DeactivateShadowModels();
        isDuringCooldown = true;
    }

    private void ActivateShadowModels()
    {
        SetShadowModelsActive(true);
        ParticleSystem fx_init = LeanPool.Spawn(fx_switch, transform.position, quaternion.identity);
        LeanPool.Despawn(fx_init, 3f);
    }

    private void DeactivateShadowModels()
    {
        SetShadowModelsActive(false);
    }

    private void SetShadowModelsActive(bool isActive)
    {
        foreach (var model in shadowModels)
        {
            model.SetActive(isActive);
        }
    }

    private void ResetHealthAndCooldown()
    {
        if (activeShadowDieFx != null)
        {
            LeanPool.Despawn(activeShadowDieFx);
            activeShadowDieFx = null;
        }

        gameObject.tag = "Player";
        ActivateShadowModels();
        healthSystem.SetHealth(healthSystem.maxHealth);
        isDuringCooldown = false;
        currentCooldown = cooldownToResetHealth;
    }
}
