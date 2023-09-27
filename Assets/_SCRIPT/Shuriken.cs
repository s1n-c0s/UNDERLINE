using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab; // ออบเจ็กต์กระสุนที่จะยิง
    public float bulletSpeed = 5f; // ความเร็วของกระสุน

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่า game object ที่ชนมี tag "Player" หรือไม่
        if (other.CompareTag("Player"))
        {
            // ให้หาตำแหน่งของผู้เล่น
            Vector3 playerPosition = other.transform.position;

            // ทำการยิงกระสุน 8 ลูกทันที
            for (int i = 0; i < 8; i++)
            {
                // สร้างออบเจ็กต์กระสุนที่ตำแหน่งของผู้เล่น
                GameObject bullet = Instantiate(bulletPrefab, playerPosition, Quaternion.identity);

                // หมุนออบเจ็กต์กระสุนเพื่อให้กระสุนถูกยิงไปทุกทิศทาง
                float angle = i * 45f;
                bullet.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // กำหนดความเร็วให้กับกระสุน
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

                // ทำลายกระสุนหลังจากเวลาที่กำหนด (เช่น 0.5 วินาที)
                Destroy(bullet, 0.5f);
            }
        }
    }
}
