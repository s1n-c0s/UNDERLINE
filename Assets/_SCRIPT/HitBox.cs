using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;

    [Header("Random")] 
    [SerializeField] private bool _isRandom;
    [SerializeField] private int[] randomDamages;
    
    [SerializeField] private int _SetDamage;
    
    private void Awake()
    {
        if (_SetDamage == 0) // If custom damage value is 0, set it to a random value
        {
            _isRandom = true;
            _SetDamage = GetRandomDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken") || other.CompareTag("Enemy"))
        {
            _healthSystem.TakeDamage(_SetDamage);
        }
    }

    private int GetRandomDamage()
    {
        int randomIndex = Random.Range(0, randomDamages.Length);
        return randomDamages[randomIndex];
    }
}