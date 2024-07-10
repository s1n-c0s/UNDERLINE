using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public int zoneOrder;
    public bool usingChance;
    public event Action<ZoneManager> OnZoneClear;

    [Serializable]
    private class EnemyPrefabs
    {
        public GameObject enemyPrefab;
        [Range(0f, 100f)]
        public float activeChancePercent;
    }

    public bool isClear;
    public bool isPlayed;

    private bool isPlayerIn = false;
    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<EnemyPrefabs> enemies;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        isClear = false;
        isPlayed = false;
        SetDoorsActive(false);
    }

    private void LateUpdate()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (activeEnemies.Count == 0 && !isClear && isPlayed)
        {
            ZoneClear();
        }
    }

    public void ActivateEnemies(bool isActive)
    {
        foreach (var enemyObj in enemies)
        {
            if (usingChance && isActive)
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                if (randomValue <= enemyObj.activeChancePercent)
                {
                    enemyObj.enemyPrefab.SetActive(true);
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
                if (isActive)
                {
                    RegisterEnemy(enemyObj.enemyPrefab);
                }
                else
                {
                    UnregisterEnemy(enemyObj.enemyPrefab);
                }
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

    private void UnregisterEnemy(GameObject enemyObj)
    {
        if (enemyObj.TryGetComponent<HealthSystem>(out HealthSystem enemy))
        {
            if (activeEnemies.Contains(enemyObj))
            {
                enemy.OnEnemyDeath -= HandleEnemyDeath;
                activeEnemies.Remove(enemyObj);
            }
        }
    }

    private void HandleEnemyDeath(HealthSystem enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath;
        activeEnemies.Remove(enemy.gameObject);

        if (activeEnemies.Count == 0)
        {
            ZoneClear();
        }
    }

    public void ZoneStart()
    {
        if (!isClear)
        {
            SetDoorsActive(true);
            ToggleDoorCollisionWithPlayer(!isPlayerIn);
        }
    }

    private void ZoneClear()
    {
        isClear = true;
        SetDoorsActive(false);
        OnZoneClear?.Invoke(this);
    }

    private void SetDoorsActive(bool isActive)
    {
        foreach (var door in doors)
        {
            door.SetActive(isActive);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<ShadowLife>())
            {
                isPlayerIn = true;
                ToggleDoorCollisionWithPlayer(false);
            }
            ZoneStart();
            isPlayed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<ShadowLife>())
        {
            isPlayerIn = false;
            ToggleDoorCollisionWithPlayer(true);
        }
    }

    private void ToggleDoorCollisionWithPlayer(bool ignoreCollision)
    {
        foreach (var door in doors)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Door"), ignoreCollision);
        }
    }
}
