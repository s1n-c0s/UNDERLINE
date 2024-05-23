using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int Damage;
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player": gameObject.SetActive(false);
                other.GetComponent<HealthSystem>().TakeDamage(Damage);
                break;
            case "Enemy": gameObject.SetActive(false);
                other.GetComponent<HealthSystem>().TakeDamage(Damage);
                break;
            case "Wall": gameObject.SetActive(false);
                break;
            default: 
                break;
        }
    }
}