using System.Collections;  // อันนี้ระเบิดพร้อมกัน
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class OilTank : MonoBehaviour
{
    [SerializeField] private int damage = 3;
    [SerializeField] private float power = 10.0f;
    [SerializeField] private float upforce = 1.0f;
    
    [Header("VFX")]
    [SerializeField] private float radius = 5.0f;
    [SerializeField] private ParticleSystem fx_expposion;
    [SerializeField] private Color drawColor = Color.yellow;
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Detonate();
            Invoke("Detonate", 5f);
        }

    }
    void Detonate()
    {
        Vector3 explosionPosition = this.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<HealthSystem>())
            {
                hit.GetComponent<HealthSystem>().TakeDamage(damage);
                ParticleSystem _fxExposion = LeanPool.Spawn(fx_expposion, hit.transform.position, hit.transform.rotation);
                LeanPool.Despawn(_fxExposion, 3f);
                CameraShake.Shake(1f, 5);
                
            }
            
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = drawColor; // ตั้งสีเพื่อแสดงรัศมีระเบิด
        Gizmos.DrawWireSphere(transform.position, radius); // วาดรูปร่างรัศมีระเบิด
    }
}



/*using System.Collections; // อันนี้ delay ศัตรูใน ระยะให้ตายช้าขึ้น
using System.Collections.Generic; 
using UnityEngine;

public class Damager : MonoBehaviour
{
    public GameObject bomb;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upforce = 1.0f;
    public GameObject explosionPrefab;

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            // Check if the collision involves a Rigidbody
            if (collision.rigidbody != null)
            {
                // Immediately explode the main game object
                Explode();
                // Delay the destruction of enemy game objects in the radius
                Invoke("Detonate", 5);
            }
        }
    }

    private void Explode()
    {
        // Set the flag to prevent multiple explosions
        hasExploded = true;

        Vector3 explosionPosition = bomb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                // Don't destroy objects with the "Player" tag
                continue;
            }
            else if (hit.CompareTag("Enemy"))
            {
                // Delay the destruction of enemy game objects and create an explosion
                Instantiate(explosionPrefab, hit.transform.position, hit.transform.rotation);
                Destroy(hit.gameObject, 1.0f); // Delayed destruction
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse);
            }
        }

        // Destroy the main game object
        Destroy(gameObject);
    }
}*/