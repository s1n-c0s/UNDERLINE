using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private E_RandomDamages randomDamagesManager;
    [SerializeField] private HealthSystem _healthSystem;

    [Header("Random Settings")]
    [SerializeField] private bool _isRandom;
    [SerializeField] private int _damage;

    [Header("VFX")]
    [SerializeField] private GameObject _fxWeakpoint;

    private void Start()
    {
        _fxWeakpoint.SetActive(false);
        if (_damage == 0)
        {
            _isRandom = true;
            _damage = randomDamagesManager.GetUnusedRandomDamage();
        }

        if (_damage >= 3)
        {
            _fxWeakpoint.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player": _healthSystem.TakeDamage(_damage);
                break;
            case "Enemy": _healthSystem.TakeDamage(1);
                break;
            case "shuriken":_healthSystem.TakeDamage(1);
                break;
        }
        
        /*if (other.CompareTag("Player") || other.CompareTag("Shuriken") || other.CompareTag("Enemy"))
        {
            _healthSystem.TakeDamage(_damage);
        }*/
    }
}
