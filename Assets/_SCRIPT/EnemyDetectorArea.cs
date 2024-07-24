using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDetectorArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> detectedEnemies = new List<GameObject>();
    private Dictionary<GameObject, int> enemyHealthBackup = new Dictionary<GameObject, int>();

    [SerializeField] private ParticleSystem[] speedlinePS;
    [SerializeField] private int PANIC_COMBO_THRESHOLD = 2;
    [SerializeField] private float PANIC_DURATION = 8f;
    [SerializeField] private float panicExtendDuration = 2f;

    private bool isInPanicMode = false;
    private float panicTimer = 0f;

    private ICombo comboManager;

    private void Start()
    {
        comboManager = FindObjectOfType<ICombo>();
        comboManager.UpdatePanicDurations(PANIC_DURATION, panicExtendDuration); // Update initial panic durations
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject enemy = other.gameObject;
            detectedEnemies.Add(enemy);

            int currentHealth = enemy.GetComponent<HealthSystem>().GetCurrentHealth();
            if (!enemyHealthBackup.ContainsKey(enemy))
            {
                enemyHealthBackup.Add(enemy, currentHealth);
            }

            if (isInPanicMode)
            {
                enemy.GetComponent<StatusManager>().ApplyStatus(StatusManager.Status.Panic, true);
                enemy.GetComponent<HealthSystem>().SetHealth(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && detectedEnemies.Contains(other.gameObject))
        {
            GameObject enemy = other.gameObject;
            detectedEnemies.Remove(enemy);
            enemyHealthBackup.Remove(enemy);
        }
    }

    public void DecreaseEnemy(GameObject enemy)
    {
        if (detectedEnemies.Contains(enemy))
        {
            detectedEnemies.Remove(enemy);

            if (!enemyHealthBackup.ContainsKey(enemy))
            {
                enemyHealthBackup.Add(enemy, enemy.GetComponent<HealthSystem>().GetCurrentHealth());
            }

            comboManager.IncreaseCombo();

            if (comboManager.GetComboCount() >= PANIC_COMBO_THRESHOLD)
            {
                if (isInPanicMode)
                {
                    ExtendPanicMode();
                }
                else
                {
                    StartPanicMode();
                }
            }
            else
            {
                EndPanicMode();
            }
        }
    }

    private void StartPanicMode()
    {
        if (!isInPanicMode)
        {
            isInPanicMode = true;
            panicTimer = 0f;

            foreach (GameObject enemy in detectedEnemies)
            {
                if (enemy != null)
                {
                    enemyHealthBackup[enemy] = enemy.GetComponent<HealthSystem>().GetCurrentHealth();
                    enemy.GetComponent<StatusManager>().ApplyStatus(StatusManager.Status.Panic, true);
                    enemy.GetComponent<HealthSystem>().SetHealth(1);
                }
            }

            foreach (var speedline in speedlinePS)
            {
                speedline.Play();
            }

            comboManager.SetPanicMode(true);
        }
    }

    private void ExtendPanicMode()
    {
        panicTimer -= panicExtendDuration;
        panicTimer = Mathf.Max(panicTimer, 0f);
    }

    private void EndPanicMode()
    {
        if (isInPanicMode)
        {
            isInPanicMode = false;

            foreach (GameObject enemy in detectedEnemies)
            {
                if (enemy != null && enemyHealthBackup.ContainsKey(enemy))
                {
                    enemy.GetComponent<StatusManager>().ApplyStatus(StatusManager.Status.Panic, false);
                    enemy.GetComponent<HealthSystem>().SetHealth(enemyHealthBackup[enemy]);
                }
            }

            enemyHealthBackup.Clear();

            foreach (var speedline in speedlinePS)
            {
                speedline.Stop();
            }

            comboManager.SetPanicMode(false);
        }
    }

    private void Update()
    {
        if (isInPanicMode)
        {
            panicTimer += Time.deltaTime;
            if (panicTimer >= PANIC_DURATION)
            {
                EndPanicMode();
            }
        }
    }

    public int GetCurrentEnemyCount()
    {
        return detectedEnemies.Count;
    }

    public bool IsInPanicMode()
    {
        return isInPanicMode;
    }
}
