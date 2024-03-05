using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private E_RandomDamages randomDamagesManager;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private bool _isRandom;
    [SerializeField] private int _damage;

    private void Awake()
    {
        if (_damage == 0)
        {
            _isRandom = true;
            _damage = randomDamagesManager.GetUnusedRandomDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player") || CompareTag("Shuriken") || CompareTag("Enemy"))
        {
            _healthSystem.TakeDamage(_damage);
        }
    }
}