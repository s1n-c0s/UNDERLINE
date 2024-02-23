using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonk : MonoBehaviour
{
    private HealthSystem _healthSystem;
    public List<GameObject> targets;
    private List<HealthSystem> _targetHealthSystems = new List<HealthSystem>();
    public float protectionDuration = 10f;
    public float cooldownDuration = 5f;
    private bool _isCooldown = false;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        CacheTargetHealthSystems();
    }

    private void CacheTargetHealthSystems()
    {
        _targetHealthSystems.Clear();
        if (targets != null)
        {
            foreach (var target in targets)
            {
                if (target != null) // Check if the target is not null
                {
                    var healthSystem = target.GetComponent<HealthSystem>();
                    if (healthSystem != null)
                    {
                        _targetHealthSystems.Add(healthSystem);
                    }
                }
            }
        }
    }
    
    void Update()
    {
        // Check if targets list is not null or empty and cooldown is not active
        if (!_isCooldown && targets != null && targets.Count > 0)
        {
            StartCoroutine(ActivateProtection());
        }
    }

    private IEnumerator ActivateProtection()
    {
        _isCooldown = true;

        ToggleProtection(true);

        yield return new WaitForSeconds(protectionDuration);

        ToggleProtection(false);

        yield return new WaitForSeconds(cooldownDuration);

        _isCooldown = false;
    }

    private void ToggleProtection(bool state)
    {
        // Check if the list of Health Systems is not null or empty
        if (_targetHealthSystems != null && _targetHealthSystems.Count > 0)
        {
            foreach (var healthSystem in _targetHealthSystems)
            {
                if (healthSystem != null && healthSystem.gameObject.activeSelf) // Check if the Health System and its GameObject are not null and active
                {
                    healthSystem.EnableProtection(state);
                }
            }
        }
    }

    private void OnDestroy()
    {
        ToggleProtection(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken") || other.CompareTag("Enemy"))
        {
            _healthSystem.TakeDamage(1);
        }
    }
}
