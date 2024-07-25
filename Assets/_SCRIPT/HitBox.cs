using System;
using Lean.Pool;
using Unity.Mathematics;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private E_RandomDamages randomDamagesManager;
    [SerializeField] private HealthSystem _healthSystem;

    [Header("Random Settings")]
    [SerializeField] private bool _isRandom;
    [SerializeField] private int _damage;

    [Header("Heal Player")]
    [SerializeField] private bool _playerHeal;
    [SerializeField] private int HealPoint = 2;
    
    [Header("VFX")]
    [SerializeField] private GameObject _fxWeakpoint;
    [SerializeField] private ParticleSystem fx_trigger;

    private void Start()
    {
        fx_trigger = randomDamagesManager.fx_trigger;
        
        _fxWeakpoint.SetActive(false);
        if (_damage == 0)
        {
            _isRandom = true;
            _damage = randomDamagesManager.GetUnusedRandomDamage();
        }

        if (_damage >= 3)
        {
            _playerHeal = true;
            _fxWeakpoint.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 objPosition = transform.position;
        objPosition.y += 2f;
        
        if (other.CompareTag("Player"))
        {
            _healthSystem.TakeDamage(_damage);
            SFXManager.instance.PlaySoundEffect(0);
            
            HealthSystem playerHP = other.GetComponent<HealthSystem>();
            if (_playerHeal && playerHP != null)
            {
                ParticleSystem fxInit = LeanPool.Spawn(fx_trigger, objPosition, Quaternion.identity);
                LeanPool.Despawn(fxInit, 3f);
                playerHP.Heal(HealPoint);
            }
        }

        if (other.CompareTag("Shuriken"))
        {
            _healthSystem.TakeDamage(1);
            SFXManager.instance.PlaySoundEffect(0);
        }
    }
}
