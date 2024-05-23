using System.Collections.Generic;
using UnityEngine;

public class OrbSystem : MonoBehaviour
{
    public float radius = 5.0f;
    public float height = 2.0f;
    public float rotationSpeed = 10.0f;
    public float activeCooldown = 5.0f;
    public List<GameObject> orbPrefabs = new List<GameObject>();

    private List<GameObject> orbs = new List<GameObject>();
    private float nextActiveTime;

    private void Start()
    {
        nextActiveTime = activeCooldown;
        SpawnOrbs();
    }

    private void Update()
    {
        RotateOrbs();
        UpdateOrbActivation();
    }

    private void RotateOrbs()
    {
        foreach (var orb in orbs)
        {
            orb?.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateOrbActivation()
    {
        nextActiveTime -= Time.deltaTime;
        if (nextActiveTime <= 0f)
        {
            ActivateOrb();
            nextActiveTime = activeCooldown;
        }
    }

    private void ActivateOrb()
    {
        if (orbs.Count > 0)
        {
            int indexToActivate = Random.Range(0, orbs.Count);
            GameObject orbToActivate = orbs[indexToActivate];

            if (!orbToActivate.activeSelf)
            {
                orbToActivate.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("No orbs to activate.");
        }
    }

    private void SpawnOrbs()
    {
        if (orbPrefabs.Count == 0)
        {
            Debug.LogWarning("No orb prefabs found.");
            return;
        }

        float angleIncrement = 360f / orbPrefabs.Count;

        for (int i = 0; i < orbPrefabs.Count; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector3 orbPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                height,
                Mathf.Sin(angle) * radius
            );

            GameObject newOrb = Instantiate(orbPrefabs[i], transform.position + orbPosition, Quaternion.identity, transform);
            newOrb.SetActive(false); // Set the orb as deactivated initially
            orbs.Add(newOrb);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f); // Draw a small sphere at the center for reference
        DrawWireCircle(transform.position, radius, height, 360);
    }

    private void DrawWireCircle(Vector3 center, float radius, float height, int segments)
    {
        float angleIncrement = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0) * radius, height, Mathf.Sin(0) * radius);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
