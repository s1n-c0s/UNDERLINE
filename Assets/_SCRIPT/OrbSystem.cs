using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class OrbSystem : MonoBehaviour
{
    public float radius = 5.0f;
    public float height = 2.0f;
    public float rotationSpeed = 10.0f;
    public float spawnCooldown = 5.0f;
    
    public GameObject orbPrefab;
    [SerializeField] private int Maxorb;
    
    private List<GameObject> orbs = new List<GameObject>(); // Initialize orbs list
    private Vector3 orbPosition;
    private float spawnTimer;

    void Start()
    {
        // Initialize spawn timer
        spawnTimer = spawnCooldown;
    }

    void Update()
    {
        RotateOrbs();
        RespawnOrbs();
    }

    void RotateOrbs()
    {
        foreach (var orb in orbs)
        {
            if (orb != null)
            {
                // Rotate the orb around the system's center
                orb.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void RespawnOrbs()
    {
        if (orbs.Count < Maxorb)
        {
            // Decrement spawn timer
            spawnTimer -= Time.deltaTime;
            // If spawn timer reaches 0 or below, spawn a new orb
            if (spawnTimer <= 0f)
            {
                SpawnOrb();
                // Reset spawn timer to spawn cooldown
                spawnTimer = spawnCooldown;
            }
        }
    }

    void SpawnOrb()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        orbPosition = new Vector3(
            Mathf.Cos(angle) * radius,
            height,
            Mathf.Sin(angle) * radius
        );
        // Instantiate the orb at the calculated position
        GameObject newOrb = LeanPool.Spawn(orbPrefab, transform.position + orbPosition, Quaternion.identity);
        // Add the spawned orb to the list
        orbs.Add(newOrb);
    }
}