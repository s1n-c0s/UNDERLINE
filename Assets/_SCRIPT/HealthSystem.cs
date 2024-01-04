using UnityEngine;
using Lean.Pool;

public class HealthSystem : MonoBehaviour
{
    public HitFlashDamage _HitFlash;
    public EnemyDetectorArea _enemyDetectorArea;
    public ParticleSystem fx_die;
    
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        if (currentHealth == 0) 
        {
            currentHealth = maxHealth;
        }

        _HitFlash = GetComponent<HitFlashDamage>();
        _enemyDetectorArea = GameObject.FindObjectOfType<EnemyDetectorArea>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (gameObject.CompareTag("Enemy"))
        {
            _HitFlash.playHitModelFX();
        }

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
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
        }
        
        /*if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }*/
    }
    
    public void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                break;
            case "Enemy":
                // update enemy count
                _enemyDetectorArea.DecreaseEnemy(gameObject);
                Destroy(gameObject);
            
                ParticleSystem fxInstance = LeanPool.Spawn(fx_die, Vector3.up + transform.position, Quaternion.identity);

                // Destroy the particle effect after 5 seconds
                LeanPool.Despawn(fxInstance, 3f);
                break;
            default:
                break;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHp()
    {
        currentHealth = maxHealth;
    }
}
