using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (other.CompareTag("Enemy"))
        {
            // ทำลาย game object ที่มี tag "Enemy"
            //Destroy(other.gameObject);*******
            Debug.Log("Shuriken Bull");

        }

        // ทำลายกระสุนหลังจากชน
        Destroy(gameObject);
    }
}
