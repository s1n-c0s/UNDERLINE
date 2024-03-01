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
        if (targets != null)
        {
            foreach (var target in targets)
            {
                if (target != null)
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
        if (!_isCooldown && targets != null && targets.Count > 0)
        {
            StartCoroutine(ActivateProtection());
        }
    }

    private IEnumerator ActivateProtection()
    {
        _isCooldown = true;
        Debug.Log("Cooldown started");
        fx_protectskill.Play();
        ToggleProtection(0, true); // Activate protection for the first enemy

        yield return new WaitForSeconds(protectionDuration);

        ToggleProtection(0, false); // Deactivate protection for the first enemy

        yield return new WaitForSeconds(cooldownDuration);

        Debug.Log("Cooldown ended");
        RemoveNullTargets(); // Remove null or missing targets from the list
        _isCooldown = false;
        fx_protectskill.Stop(); // Stop particle effect when cooldown ends
    }

    private void ToggleProtection(int index, bool state)
    {
        if (_targetHealthSystems.Count > 0 && index < _targetHealthSystems.Count)
        {
            if (_targetHealthSystems[index] != null)
            {
                _targetHealthSystems[index].EnableProtection(state);
            }
        }
    }

    private void RemoveNullTargets()
    {
        _targetHealthSystems.RemoveAll(item => item == null); // Remove null or missing targets
    }

    private void OnDestroy()
    {
        // Deactivate protection for all enemies when this enemy is destroyed
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
