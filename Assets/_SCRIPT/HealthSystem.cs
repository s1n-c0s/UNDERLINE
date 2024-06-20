using System;
using UnityEngine;
using Lean.Pool;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private EnemyDetectorArea _enemyDetectorArea;
    [SerializeField] private StatusManager _statusManager;

    [Header("Health Point")]
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isProtected;
    
    public event Action<HealthSystem> OnEnemyDeath;

    [Header("VFX")]
    public ParticleSystem fx_die;
    [SerializeField] private GameObject fx_attackhit;
    [SerializeField] private GameObject fx_block;

    private void Start()
    {
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
        _enemyDetectorArea = FindObjectOfType<EnemyDetectorArea>();
        _statusManager = GetComponent<StatusManager>();
    }

    public void SetProtection(bool status)
    {
        isProtected = status;
    }

    public void TakeDamage(int damage)
    {
        if (isProtected || damage == 0)
        {
            PlayHitAttack(true);
            return;
        }

        currentHealth -= damage;
        if (!CompareTag("Player"))
        {
            PlayHitAttack(false);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void PlayHitAttack(bool isBlock)
    {
        if (isBlock)
        {
            GameObject fxInstance = LeanPool.Spawn(fx_block, new Vector3(0, 4, 0) + transform.position, Quaternion.identity);
            LeanPool.Despawn(fxInstance, 3f);
        }
        else
        {
            GameObject fxInstance = LeanPool.Spawn(fx_attackhit, new Vector3(0, 2, 0) + transform.position, Quaternion.identity);
            LeanPool.Despawn(fxInstance, 3f);
        }
    }

    public void Heal(int heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
    }
    
    public void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                // Handle player death
                break;
            case "Enemy":
                _enemyDetectorArea.DecreaseEnemy(gameObject);
                
                OnEnemyDeath?.Invoke(this);
                Destroy(gameObject);
                CameraShake.Shake(0.6f, 5);
                var fxInstance = LeanPool.Spawn(fx_die, Vector3.up + transform.position, Quaternion.identity);
                LeanPool.Despawn(fxInstance, 3f);
                break;
            case "Wall":
                Destroy(gameObject);
                break;
        }
    }

    public void SetHealth(int num)
    {
        currentHealth = num;
    }

    public int GetCurrentHealth() => currentHealth;

    public void ResetHp()
    {
        currentHealth = maxHealth;
    }
}
