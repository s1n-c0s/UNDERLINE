using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class OilTank : MonoBehaviour
{
    [SerializeField] private bool isFireTank;
    [SerializeField] private GameObject FireArea;
    [SerializeField] private int damage = 3;
    [SerializeField] private float power = 10.0f;
    [SerializeField] private float upforce = 1.0f;
    [SerializeField] private float forceRadius = 20.0f;
    [SerializeField] private float checkRadius = 7.0f;
    [SerializeField] private float checkInterval = 0.5f; // Check every 0.5 seconds

    [Header("VFX")]
    [SerializeField] private List<GameObject> fx_Bombs;
    [SerializeField] private ParticleSystem fx_Explosion;
    [SerializeField] private Color drawColor = Color.yellow;

    private bool hasExploded = false;
    private float nextCheckTime = 0f;

    private void Start()
    {
        foreach (var fxBomb in fx_Bombs)
        {
            fxBomb.SetActive(false);
        }
    }

    private void Update()
    {
        if (!hasExploded && Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            CheckForFireObjects();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            TriggerExplosion(checkRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shuriken"))
        {
            TriggerExplosion(checkRadius);
        }
    }

    private void TriggerExplosion(float radius)
    {
        if (hasExploded) return;

        hasExploded = true;

        if (isFireTank)
        {
            fx_Bombs[0].SetActive(true);
            StartCoroutine(ExplosionCoroutine(FireBomb, 2f, radius));
        }
        else
        {
            fx_Bombs[1].SetActive(true);
            StartCoroutine(ExplosionCoroutine(Detonate, 2f, radius));
        }
    }

    private IEnumerator ExplosionCoroutine(System.Action<float> explosionMethod, float delay, float radius)
    {
        yield return new WaitForSeconds(delay);
        explosionMethod(radius);
    }

    private void Detonate(float radius)
    {
        ApplyExplosion(transform.position, Quaternion.identity, true, radius);
        Destroy(gameObject);
    }

    private void FireBomb(float radius)
    {
        Quaternion randomYRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        ApplyExplosion(transform.position, randomYRotation, false, radius);

        GameObject fireObj = Instantiate(FireArea, transform.position, Quaternion.identity);
        Destroy(fireObj, 4f);
        Destroy(gameObject);
    }

    private void ApplyExplosion(Vector3 position, Quaternion rotation, bool applyDamage, float checkRadius)
    {
        CheckNearbyOilTanks(position, checkRadius);
        ApplyForceToNearbyObjects(position, applyDamage);
        SpawnExplosionEffects(position, rotation);
    }

    private void CheckNearbyOilTanks(Vector3 position, float checkRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("OilTank") && !hit.GetComponent<OilTank>().hasExploded)
            {
                hit.GetComponent<OilTank>().TriggerExplosion(checkRadius);
            }
        }
    }

    private void CheckForFireObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Fire"))
            {
                TriggerExplosion(checkRadius);
                break; // Exit the loop as we only need one fire object to trigger the explosion
            }
        }
    }

    private void ApplyForceToNearbyObjects(Vector3 position, bool applyDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(position, forceRadius);
        foreach (Collider hit in colliders)
        {
            if (applyDamage)
            {
                ApplyDamage(hit);
            }

            if (hit.attachedRigidbody != null)
            {
                ApplyExplosionForce(hit.attachedRigidbody, position);
            }
        }
    }

    private void ApplyDamage(Collider hit)
    {
        var healthSystem = hit.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.TakeDamage(damage);
        }
    }

    private void ApplyExplosionForce(Rigidbody rb, Vector3 explosionPosition)
    {
        rb.AddExplosionForce(power, explosionPosition, forceRadius, upforce, ForceMode.Impulse);
    }

    private void SpawnExplosionEffects(Vector3 position, Quaternion rotation)
    {
        ParticleSystem explosionEffect = LeanPool.Spawn(fx_Explosion, position, rotation);
        LeanPool.Despawn(explosionEffect, 3f);
        CameraShake.Shake(1f, 5);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = drawColor; // Set color to display explosion radii
        Gizmos.DrawWireSphere(transform.position, forceRadius); // Draw the force radius
        Gizmos.color = Color.red; // Change color to distinguish check radius
        Gizmos.DrawWireSphere(transform.position, checkRadius); // Draw the check radius
    }
}
