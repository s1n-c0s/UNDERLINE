using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shuriken"))
        {
            StartCoroutine(ShootBullets());
        }
    }

    IEnumerator ShootBullets()
    {
        // สร้างกระสุน 8 ลูกและยิงออกไปรอบตัวเอง
        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            // ตั้งความเร็วและทิศทางของกระสุน
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 10f; // แก้ตามความเหมาะสม
            Destroy(bullet, 2f); // ลบกระสุนหลังจาก 2 วินาที
            yield return new WaitForSeconds(0.2f); // รอ 0.2 วินาทีระหว่างการยิงกระสุน
        }
    }
}
