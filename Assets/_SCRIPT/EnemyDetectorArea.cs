using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> detectedEnemies = new List<GameObject>();
    private Dictionary<GameObject, int> enemyHealthBackup = new Dictionary<GameObject, int>();

    [SerializeField] private ParticleSystem[] _speedlinePS;

    private const int PANIC_COMBO_THRESHOLD = 2;
    private const float PANIC_DURATION = 3f;

    private bool isInPanicMode = false;
    private float panicTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            detectedEnemies.Add(other.gameObject);
            int currentHealth = other.gameObject.GetComponent<HealthSystem>().GetCurrentHealth();
            if (!enemyHealthBackup.ContainsKey(other.gameObject))
            {
                enemyHealthBackup.Add(other.gameObject, currentHealth);
            }
            if (isInPanicMode)
            {
                other.gameObject.GetComponent<HealthSystem>().SetHealth(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && detectedEnemies.Contains(other.gameObject))
        {
            detectedEnemies.Remove(other.gameObject);
            enemyHealthBackup.Remove(other.gameObject);
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

            ICombo.Instance.IncreaseCombo();

            if (ICombo.Instance.hitcombo >= PANIC_COMBO_THRESHOLD)
            {
                StartPanicMode();
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
                    enemy.GetComponent<HealthSystem>().SetHealth(1);
                }
            }
            foreach (var speedline in _speedlinePS)
            {
                speedline.Play();
            }
        }
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
                    enemy.GetComponent<HealthSystem>().SetHealth(enemyHealthBackup[enemy]);
                }
            }
            enemyHealthBackup.Clear();
            foreach (var speedline in _speedlinePS)
            {
                speedline.Stop();
            }
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
    
    public int GetCurrentEnemy()
    {
        return detectedEnemies.Count;
    }
}
