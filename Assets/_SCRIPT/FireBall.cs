using Lean.Pool;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    //[SerializeField] private int fireDamage;

    [Header("VFX")]
    [SerializeField] private ParticleSystem fx_hit;

    [Header("Burn Effect")]
    [SerializeField] private float burnDuration = 2f;
    [SerializeField] private int burnDamagePerSecond = 1;
    

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        StatusManager statusManager = other.GetComponent<StatusManager>();

        if (healthSystem != null)
        {
            //healthSystem.TakeDamage(fireDamage);
            if (statusManager != null)
            {
                statusManager.ApplyStatus(StatusManager.Status.Burn, true, burnDamagePerSecond, burnDuration);
            }
            PlayHitEffect();
            LeanPool.Despawn(gameObject);
            //gameObject.SetActive(false);
            return;
        }

        if (other.CompareTag("Wall"))
        {
            PlayHitEffect();
            LeanPool.Despawn(gameObject);
            return;
        }

        // Additional handling for other cases can be added here
    }

    private void PlayHitEffect()
    {
        if (fx_hit != null)
        {
            ParticleSystem hitEffect = Instantiate(fx_hit, transform.position, Quaternion.identity);
            hitEffect.Play();
            Destroy(hitEffect.gameObject, hitEffect.main.duration);
        }
    }
}