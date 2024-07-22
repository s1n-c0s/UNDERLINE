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

    public enum ZoneState
    {
        NotStarted,
        Active,
        Cleared
    }

    public ZoneState currentState = ZoneState.NotStarted;

    private bool isPlayerIn = false;
    [SerializeField] private List<GameObject> doors;
    [SerializeField] private List<EnemyPrefabs> enemies;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        SetDoorsActive(false);
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

        if (activeEnemies.Count == 0 && currentState != ZoneState.Cleared)
        {
            ZoneClear();
        }
    }

    public void ZoneStart()
    {
        if (currentState == ZoneState.NotStarted)
        {
            SetDoorsActive(true);
            StartCoroutine(ToggleDoorCollisionWithPlayer(!isPlayerIn, 0.75f));
            currentState = ZoneState.Active;
        }
    }

    private void ZoneClear()
    {
        currentState = ZoneState.Cleared;
        FadeOutDoors();   
        OnZoneClear?.Invoke(this);
    }

    private void SetDoorsActive(bool isActive)
    {
        foreach (var door in doors)
        {
            door.SetActive(isActive);
            shaderFadeWall fadeController = door.GetComponent<shaderFadeWall>();
            if (fadeController != null)
            {
                if (isActive)
                {
                    fadeController.StartFadeIn();
                }
                else
                {
                    fadeController.ResetScale();
                    fadeController.enabled = false;
                }
            }
        }
    }

    private void FadeOutDoors()
    {
        foreach (var door in doors)
        {
            shaderFadeWall fadeController = door.GetComponent<shaderFadeWall>();
            if (fadeController != null)
            {
                fadeController.OnFadeOutComplete += HandleFadeOutComplete;
                fadeController.StartFadeOut();
            }
        }
    }

    private void HandleFadeOutComplete(shaderFadeWall sender)
    {
        sender.OnFadeOutComplete -= HandleFadeOutComplete;
        sender.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<ShadowLife>())
            {
                isPlayerIn = true;
                StartCoroutine(ToggleDoorCollisionWithPlayer(false, 0.75f));
            }
            if (currentState == ZoneState.NotStarted)
            {
                ZoneStart();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<ShadowLife>())
        {
            isPlayerIn = false;
            StartCoroutine(ToggleDoorCollisionWithPlayer(true, 0.75f));
        }
    }

    private IEnumerator ToggleDoorCollisionWithPlayer(bool ignoreCollision, float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var door in doors)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Door"), ignoreCollision);
        }
    }
}