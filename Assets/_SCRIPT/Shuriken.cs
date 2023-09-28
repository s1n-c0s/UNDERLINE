/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab; // ออบเจ็กต์กระสุนที่จะยิง
    public float bulletSpeed = 5f; // ความเร็วของกระสุน
    public float bulletSpawnRadius = 3f; // รัศมีของรัศมีการสร้างกระสุน
    public float timeForDestroy1 = 0.5f;

    //private bool canDamage = true; // ตัวแปรเพื่อกำหนดว่ากระสุนสามารถทำลาย game object ที่มี tag "Player" หรือไม่

    private void OnTriggerEnter(Collider other)
    {


        // ตรวจสอบว่า game object ที่ชนมี tag "Player" หรือไม่
        if (other.CompareTag("Player"))
        {
            // หาตำแหน่งของผู้เล่น
            Vector3 playerPosition = other.transform.position;

            // สร้างรัศมีของกระสุนเป็นรูปร่างวงกลม
            float radius = bulletSpawnRadius;

            // ทำการสร้างกระสุนตามระนาบรอบตัวผู้เล่น
            for (float angle = 0; angle < 360; angle += 45)
            {
                // คำนวณตำแหน่งของกระสุนบนระนาบ x-z
                float x = playerPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = playerPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                // สร้างออบเจ็กต์กระสุนที่ตำแหน่งใหม่
                GameObject bullet = Instantiate(bulletPrefab, new Vector3(x, playerPosition.y, z), Quaternion.identity);

                // ปรับความเร็วของกระสุนให้มันพุ่งออกจากวงและห่างจากตัวผู้เล่น
                Vector3 bulletDirection = (bullet.transform.position - playerPosition).normalized;
                bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;

                // ทำลายกระสุนหลังจากเวลาที่กำหนด (เช่น 0.5 วินาที)
                Destroy(bullet, timeForDestroy1);
            }
        }

   
       
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab; // ออบเจ็กต์กระสุนที่จะยิง
    public float bulletSpeed = 5f; // ความเร็วของกระสุน
    public float bulletSpawnRadius = 3f; // รัศมีของรัศมีการสร้างกระสุน
    public float timeForDestroy1 = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่า game object ที่ชนมี tag "Player" หรือ "Shuriken"
        if (other.CompareTag("Player") || other.CompareTag("Shuriken"))
        {
            // หาตำแหน่งของผู้เล่นหรือ "Shuriken" ที่ชนกัน
            Vector3 targetPosition = other.transform.position;

            // สร้างรัศมีของกระสุนเป็นรูปร่างวงกลม
            float radius = bulletSpawnRadius;

            // ทำการสร้างกระสุนตามระนาบรอบตัวผู้เล่นหรือ "Shuriken"
            for (float angle = 0; angle < 360; angle += 45)
            {
                // คำนวณตำแหน่งของกระสุนบนระนาบ x-z
                float x = targetPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = targetPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                // สร้างออบเจ็กต์กระสุนที่ตำแหน่งใหม่
                GameObject bullet = Instantiate(bulletPrefab, new Vector3(x, targetPosition.y, z), Quaternion.identity);

                // ปรับความเร็วของกระสุนให้มันพุ่งออกจากวงและห่างจากเป้าหมาย
                Vector3 bulletDirection = (bullet.transform.position - targetPosition).normalized;
                bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;

                // ทำลายกระสุนหลังจากเวลาที่กำหนด (เช่น 0.5 วินาที)
                Destroy(bullet, timeForDestroy1);
            }

            // ทำลายตัวเองหลังจากสร้างกระสุนเสร็จสิ้น
            Destroy(gameObject);
        }
    }
}