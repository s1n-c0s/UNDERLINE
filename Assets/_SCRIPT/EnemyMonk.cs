using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonk : MonoBehaviour
{
    private HealthSystem _healthSystem;
    public List<GameObject> targets;
    private List<HealthSystem> _targetHealthSystems = new List<HealthSystem>();
    
    [Header("Protect Skill")]
    public float protectionDuration = 10f;
    public float cooldownDuration = 5f;
    private bool _isCooldown = false;
    [SerializeField] private ParticleSystem fx_protectskill;

    private void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        CacheTargetHealthSystems();
    }

    private void CacheTargetHealthSystems()
    {
        _targetHealthSystems.Clear();
        foreach (var target in targets)
        {
            var healthSystem = target?.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                _targetHealthSystems.Add(healthSystem);
            }
        }
    }

    void Update()
    {
        RemoveNullTargets(); // Clean up before checking to start protection
        if (!_isCooldown && targets.Count > 0)
        {
            StartCoroutine(ActivateProtection());
        }
    }
    
    private IEnumerator ActivateProtection()
    {
        _isCooldown = true;
        Debug.Log("Cooldown started");
        fx_protectskill.Play();
    
        yield return new WaitForSeconds(protectionDuration);
    
        RemoveNullTargets(); // Ensure the list is clean before toggling protection
        // Assuming fx_protect is managed inside the EnableProtection method of HealthSystem
        ToggleProtection(0, true); // Activate protection for the first enemy
        fx_protectskill.Stop(); // Stop the charging effect
    
        yield return new WaitForSeconds(cooldownDuration);
    
        ToggleProtection(0, false); // Deactivate protection for the first enemy
        Debug.Log("Cooldown ended");
    
        _isCooldown = false;
    }


    private void ToggleProtection(int index, bool state)
    {
        if (index < _targetHealthSystems.Count)
        {
            _targetHealthSystems[index]?.EnableProtection(state);
        }
    }

    private void RemoveNullTargets()
    {
        targets.RemoveAll(item => item == null); // Clean up GameObject list
        _targetHealthSystems.RemoveAll(item => item == null || item.gameObject == null); // Clean up HealthSystem list, checking both the component and its GameObject
    }


    private void OnDestroy()
    {
        for (int i = 0; i < _targetHealthSystems.Count; i++)
        {
            ToggleProtection(i, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken") || other.CompareTag("Enemy"))
        {
            _healthSystem.TakeDamage(1);
        }
    }
}
