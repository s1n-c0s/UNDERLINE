using System;
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
    [SerializeField] private float radius = 5.0f;

    [Header("VFX")]
    [SerializeField] private List<GameObject> fx_bombs;
    [SerializeField] private ParticleSystem fx_expposion;
    [SerializeField] private Color drawColor = Color.yellow;

    private void Start()
    {
        /*foreach (var fxBomb in fx_bombs)
        {
            fxBomb.SetActive(false);
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (isFireTank)
            {
                //fx_bombs[0].SetActive(true);
                StartCoroutine(FireBombCoroutine(2f));
            }
            else
            {
                //fx_bombs[1].SetActive(true);
                StartCoroutine(DetonateCoroutine(2f));
            }
        }
    }

    private IEnumerator DetonateCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Detonate();
    }

    private IEnumerator FireBombCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        FireBomb();
    }

    private void Detonate()
    {
        ApplyExplosion(transform.position, Quaternion.identity, true);
        Destroy(gameObject);
    }

    private void FireBomb()
    {
        Quaternion randomYRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        ApplyExplosion(transform.position, randomYRotation, false);

        GameObject fireObj = Instantiate(FireArea, transform.position, Quaternion.identity);
        Destroy(fireObj, 4f);
        Destroy(gameObject);
    }

    private void ApplyExplosion(Vector3 position, Quaternion rotation, bool applyDamage)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider hit in colliders)
        {
            if (applyDamage && hit.GetComponent<HealthSystem>())
            {
                hit.GetComponent<HealthSystem>().TakeDamage(damage);
                HandleExplosionEffects(hit.transform.position, hit.transform.rotation);
            }

            if (hit.attachedRigidbody != null)
            {
                ApplyExplosionForce(hit.attachedRigidbody, position);
            }
        }

        HandleExplosionEffects(position, rotation);
    }

    private void ApplyExplosionForce(Rigidbody rb, Vector3 explosionPosition)
    {
        rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
    }

    private void HandleExplosionEffects(Vector3 position, Quaternion rotation)
    {
        ParticleSystem explosionEffect = LeanPool.Spawn(fx_expposion, position, rotation);
        LeanPool.Despawn(explosionEffect, 3f);
        CameraShake.Shake(1f, 5);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = drawColor; // Set color to display explosion radius
        Gizmos.DrawWireSphere(transform.position, radius); // Draw the explosion radius shape
    }
}
