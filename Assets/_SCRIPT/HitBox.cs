using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private bool _isRandom;
    [SerializeField] private int[] _randomDamages;
    [SerializeField] private int _damage;

    private static HashSet<int> _usedDamages = new HashSet<int>();

    private void Awake()
    {
        if (_damage == 0)
        {
            _isRandom = true;
            _damage = GetRandomDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTargetCollider(other))
        {
            _healthSystem.TakeDamage(_damage);
        }
    }

    private int GetRandomDamage()
    {
        if (_isRandom)
        {
            List<int> availableDamages = new List<int>();

            foreach (int damage in _randomDamages)
            {
                if (!_usedDamages.Contains(damage))
                {
                    availableDamages.Add(damage);
                }
            }

            if (availableDamages.Count == 0)
            {
                // All damages have been used, reset the used damages set
                _usedDamages.Clear();
                availableDamages.AddRange(_randomDamages);
            }

            int randomIndex = Random.Range(0, availableDamages.Count);
            int randomDamage = availableDamages[randomIndex];
            _usedDamages.Add(randomDamage);
            return randomDamage;
        }
        else
        {
            return _damage;
        }
    }


    private bool IsTargetCollider(Collider collider)
    {
        return collider.CompareTag("Player") || collider.CompareTag("Shuriken") || collider.CompareTag("Enemy");
    }
}