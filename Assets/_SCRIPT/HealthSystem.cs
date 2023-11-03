using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public EnemyDetectorArea _enemyDetectorArea;
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            /*Destroy(gameObject);*/
            //Debug.Log("Character is dead.");
        }
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
            _enemyDetectorArea.DecreseEnemy();
        }

        // destroy the enemy object
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
