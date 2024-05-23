using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int _fireDamage;

    [Header("VFX")] [SerializeField] private ParticleSystem fx_fire;
    
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player": gameObject.SetActive(false);
                other.GetComponent<HealthSystem>().TakeDamage(_fireDamage);
                break;
            case "Enemy": 
                if (other.GetComponent<HealthSystem>())
                {
                    gameObject.SetActive(false);
                    other.GetComponent<HealthSystem>().TakeDamage(_fireDamage);
                }
                /*else
                { 
                    other.GetComponent<HitBox>().fireBallHit(_fireDamage);
                }*/
                break;
            case "Wall": gameObject.SetActive(false);
                break;
            default: 
                break;
        }
    }
}