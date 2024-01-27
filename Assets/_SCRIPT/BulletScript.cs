using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class BulletScript : MonoBehaviour
{
    //private bool canDamage = true; // ตัวแปรเพื่อกำหนดว่ากระสุนสามารถทำลาย game object ที่มี tag "Player" หรือไม่

    /*public void SetCanDamage(bool canDamageValue)
    {
        canDamage = canDamageValue;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่ากระสุนสามารถทำลาย game object ที่มี tag "Enemy" หรือไม่
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            // ทำลาย game object ที่มี tag "Enemy"
            if (other.gameObject.GetComponent<HealthSystem>())
            {
                other.GetComponent<HealthSystem>().TakeDamage(1);
            }
            LeanPool.Despawn(gameObject);
        }
        // ทำลายกระสุนหลังจากชน
      
    }
}
