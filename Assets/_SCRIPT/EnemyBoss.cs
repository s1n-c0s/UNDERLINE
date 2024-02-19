using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private HealthSystem _HealthSystem; // Allow setting this in the Inspector
    [SerializeField] private EnemyDetectorArea _enemyDetectorArea; // Prefer setting this in the Inspector as well

    private void Awake()
    {
        // Validate the components are properly assigned
        if (_HealthSystem == null) _HealthSystem = GetComponent<HealthSystem>();
        if (_enemyDetectorArea == null) _enemyDetectorArea = FindObjectOfType<EnemyDetectorArea>();

        if (_HealthSystem == null)
        {
            Debug.LogError("HealthSystem component not found on " + gameObject.name);
        }

        if (_enemyDetectorArea == null)
        {
            Debug.LogError("EnemyDetectorArea component not found in the scene.");
        }
    }

    private void OnEnable()
    {
        // Safely subscribe to the event
        if (_enemyDetectorArea != null)
        {
            _enemyDetectorArea.OnEnemyDecreased += BossTakeDamage;
        }
    }

    private void OnDisable()
    {
        // Safely unsubscribe from the event
        if (_enemyDetectorArea != null)
        {
            _enemyDetectorArea.OnEnemyDecreased -= BossTakeDamage;
        }
    }

    private void BossTakeDamage()
    {
        // Check for null in case this is called unexpectedly
        if (_HealthSystem != null)
        {
            _HealthSystem.TakeDamage(10); // Assuming TakeDamage is a method in HealthSystem
        }
        else
        {
            Debug.LogWarning("HealthSystem not set for " + gameObject.name);
        }
    }
}