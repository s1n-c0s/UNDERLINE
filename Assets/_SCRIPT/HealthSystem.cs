using Unity.Mathematics;
using UnityEngine;
using Lean.Pool;

public class HealthSystem : MonoBehaviour
{
    public EnemyDetectorArea _enemyDetectorArea;
    public ParticleSystem fx_die;
    
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        
        _enemyDetectorArea = GameObject.FindGameObjectWithTag("EnemyDetector").GetComponent<EnemyDetectorArea>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 )
        {
            currentHealth = 0;
            Die();
            /*Destroy(gameObject);*/
            //Debug.Log("Character is dead.");
        }

        /*if (currentHealth < 0 && gameObject.CompareTag("Player"))
        {
            currentHealth = 0;
            Die();
        }*/
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    
    public void Die()
    {
        // other code for handling death
        if (gameObject.CompareTag("Enemy"))
        {
            // update enemy count
            _enemyDetectorArea.DecreaseEnemy();
        }

        // Instantiate the particle effect
        ParticleSystem fxInstance = LeanPool.Spawn(fx_die, Vector3.up + transform.position, quaternion.identity);

        // Destroy the particle effect after 5 seconds
        LeanPool.Despawn(fxInstance.gameObject, 3f);
        
        // destroy the enemy object
        if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
