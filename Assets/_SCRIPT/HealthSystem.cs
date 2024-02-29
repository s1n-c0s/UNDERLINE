using UnityEngine;
using Lean.Pool;

public class HealthSystem : MonoBehaviour
{
    /*public HitFlashDamage _HitFlash;*/
    [SerializeField] private EnemyDetectorArea _enemyDetectorArea;
    
    [Header("Health Point")]
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool hasProtect;

    [Header("VFX")] 
    public ParticleSystem fx_die;
    [SerializeField] private ParticleSystem fx_attackhit;
    
    void Start()
    {
        if (currentHealth == 0) 
        {
            currentHealth = maxHealth;
        }

        /*_HitFlash = GetComponent<HitFlashDamage>();*/
        _enemyDetectorArea = GameObject.FindObjectOfType<EnemyDetectorArea>();
    }
    
    public void EnableProtection(bool status)
    {
        hasProtect = status;
    }


    public void TakeDamage(int damage)
    {
        if (!hasProtect)
        {
            currentHealth -= damage;
            if (!CompareTag("Player"))
            {
                playHitAttack();
            }
            
            if (currentHealth <= 0 )
            {
                currentHealth = 0;
                Die();
            }
        }
    }

    private void playHitAttack()
    {
        ParticleSystem fxInstance = LeanPool.Spawn(fx_attackhit, new Vector3(0,2,0) + transform.position, Quaternion.identity);

        // Destroy the particle effect after 5 seconds
        LeanPool.Despawn(fxInstance, 3f);
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
        if (gameObject.CompareTag("Enemy"))
        {
            // update enemy count
            _enemyDetectorArea.DecreaseEnemy(gameObject);
            Destroy(gameObject);
            CameraShake.Shake(0.5f, 2);
            
            ParticleSystem fxInstance = LeanPool.Spawn(fx_die, Vector3.up + transform.position, Quaternion.identity);

            // Destroy the particle effect after 5 seconds
            LeanPool.Despawn(fxInstance, 3f);
        }
        /*switch (gameObject.tag)
        {
            case "Player":
                break;
            case "Enemy":
                // update enemy count
                _enemyDetectorArea.DecreaseEnemy(gameObject);
                Destroy(gameObject);
                CameraShake.Shake(0.5f, 2);
            
                ParticleSystem fxInstance = LeanPool.Spawn(fx_die, Vector3.up + transform.position, Quaternion.identity);

                // Destroy the particle effect after 5 seconds
                LeanPool.Despawn(fxInstance, 3f);
                break;
            default:
                break;
        }*/
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
