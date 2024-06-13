using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public int zoneOrder; // Add this property
    public bool usingChance; // Add this property
    public event Action<ZoneManager> OnZoneClear;

    [Serializable]
    private class EnemyPrefabs
    {
        public GameObject enemyPrefab;
        [Range(0f, 100f)]
        public float activeChancePercent; // Chance in percent
    }

    public bool isClear;
    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<EnemyPrefabs> Enemys;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        isClear = false;
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (activeEnemies.Count == 0 && !isClear)
        {
            ZoneClear();
        }
    }

    public void ActivateEnemies(bool isActive)
    {
        foreach (EnemyPrefabs enemyObj in Enemys)
        {
            if (usingChance)
            {
                // Convert percentage chance to a probability
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                if (randomValue <= enemyObj.activeChancePercent)
                {
                    enemyObj.enemyPrefab.SetActive(isActive);
                    RegisterEnemy(enemyObj.enemyPrefab);
                }
                else
                {
                    enemyObj.enemyPrefab.SetActive(false);
                }
            }
            else
            {
                enemyObj.enemyPrefab.SetActive(isActive);
                RegisterEnemy(enemyObj.enemyPrefab);
            }
        }
    }

    private void RegisterEnemy(GameObject enemyObj)
    {
        if (enemyObj.TryGetComponent<HealthSystem>(out HealthSystem enemy))
        {
            if (!activeEnemies.Contains(enemyObj))
            {
                activeEnemies.Add(enemyObj);
                enemy.OnEnemyDeath += HandleEnemyDeath;
            }
        }
    }

    private void HandleEnemyDeath(HealthSystem enemy)
    {
        // Unsubscribe from the event to prevent memory leaks
        enemy.OnEnemyDeath -= HandleEnemyDeath;

        // Remove the enemy from the list
        activeEnemies.Remove(enemy.gameObject);

        // Check if all enemies are cleared
        if (activeEnemies.Count == 0)
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
            
            ActivateEnemies(true);
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
