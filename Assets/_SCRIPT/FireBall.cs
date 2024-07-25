using System;
using Cinemachine.Utility;
using Lean.Pool;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f; // Assign a default lifetime if not set in the Inspector
    [SerializeField] private bool isArea = false;
    [SerializeField] private float despawnRadius = 15f; // Radius for despawn detection
    
    [Header("Burn Effect")]
    [SerializeField] private float burnDuration = 2f;
    [SerializeField] private int burnDamagePerSecond = 1;

    private Vector3 initialPosition;
    private Vector3 lastPosition;

    private void Start()
    {
        if (!isArea)
        {
            initialPosition = transform.position;
            lastPosition = transform.position;
        }
    }

    private void Update()
    {
        if (!isArea)
        {
            if (Vector3.Distance(initialPosition, transform.position) > despawnRadius)
            {
                afterShoot();
            }
        }
    }

    private void afterShoot()
    {
        LeanPool.Despawn(gameObject, lifetime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        StatusManager statusManager = other.GetComponent<StatusManager>();

        if (healthSystem != null)
        {
            if (statusManager != null)
            {
                statusManager.ApplyStatus(StatusManager.Status.Burn, true, burnDamagePerSecond, burnDuration);
            }
            if (!isArea)
            {
                LeanPool.Despawn(gameObject);
            }
        }
    }
}