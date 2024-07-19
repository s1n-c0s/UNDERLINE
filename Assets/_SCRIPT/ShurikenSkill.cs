using System;
using Lean.Pool;
using UnityEngine;

public class ShurikenSkill : MonoBehaviour
{
    private HealthSystem _healthSystem;

    [SerializeField] private float despawnTimer = 3f;
    [SerializeField] private int rightOffset = 10; // Default horizontal offset value
    [SerializeField] private int heightOffset = 2; // Default vertical offset value
    [SerializeField] private float speed = 30f;
    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private bool canCombo = true;

    private bool ActiveSkill = false;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canCombo && _healthSystem.GetCurrentHealth() > 0)
            {
                _healthSystem.TakeDamage(1);
                canCombo = false;
                ActiveSkill = true;
            }
        }
    }

    private void SpawnShuriken()
    {
        Vector3 spawnPosition = transform.position + new Vector3(rightOffset, heightOffset, 0);
        GameObject shuriken = LeanPool.Spawn(shurikenPrefab, spawnPosition, Quaternion.identity);
        
        ShurikenMovement shurikenMovement = shuriken.GetComponent<ShurikenMovement>();
        if (shurikenMovement != null)
        {
            shurikenMovement.SetSpeed(speed);
        }

        LeanPool.Despawn(shuriken, despawnTimer);
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
        if (ActiveSkill)
        {
            SpawnShuriken();
            ActiveSkill = false;
        }
        if (!canCombo)
        {
            canCombo = true;
        }
    }
}