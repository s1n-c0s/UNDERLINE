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