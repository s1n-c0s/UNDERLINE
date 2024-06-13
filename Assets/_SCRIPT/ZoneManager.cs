using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public int zoneOrder; // Add this property
    public event Action<ZoneManager> OnZoneClear;
    
    public bool isClear;
    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<GameObject> Enemys;

    private void Start()
    {
        isClear = false;
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }

        // Subscribe to enemy death events and deactivate them initially
        foreach (GameObject enemyObj in Enemys)
        {
            if (enemyObj.TryGetComponent<HealthSystem>(out HealthSystem enemy))
            {
                enemy.OnEnemyDeath += HandleEnemyDeath;
            }
        }
    }

    private void LateUpdate()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (Enemys.Count == 0 && !isClear)
        {
            ZoneClear();
        }
    }

    public void ActivateEnemies(bool isActive)
    {
        foreach (GameObject activeEnemy in Enemys)
        {
            activeEnemy.SetActive(isActive);
        }
    }

    private void HandleEnemyDeath(HealthSystem enemy)
    {
        // Unsubscribe from the event to prevent memory leaks
        enemy.OnEnemyDeath -= HandleEnemyDeath;

        // Remove the enemy from the list
        Enemys.Remove(enemy.gameObject);

        // Check if all enemies are cleared
        if (Enemys.Count == 0)
        {
            ZoneClear();
        }
    }

    public void ZoneStart()
    {
        if (!isClear)
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(true);
            }
        }
    }

    private void ZoneClear()
    {
        isClear = true;
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
        OnZoneClear?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ZoneStart();
        }
    }
}
