﻿/*using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float bulletSpawnRadius = 3f;
    public float timeForDestroy = 0.5f;
    public int numBullets = 8;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken"))
        {
            Vector3 targetPosition = other.transform.position;
            float radius = bulletSpawnRadius;
            float angleIncrement = 360f / numBullets;

            for (int i = 0; i < numBullets; i++)
            {
                float angle = i * angleIncrement;
                float x = targetPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = targetPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                GameObject bullet = Instantiate(bulletPrefab, new Vector3(x, targetPosition.y, z), Quaternion.identity);

                Vector3 bulletDirection = (bullet.transform.position - targetPosition).normalized;

                // ใช้ Quaternion.LookRotation เพื่อหมุนโมเดลให้ชี้ไปทางทิศทางของแรง
                Quaternion rotation = Quaternion.LookRotation(bulletDirection);
                bullet.transform.rotation = rotation;

                bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
                Destroy(bullet, timeForDestroy);
            }

            Destroy(gameObject);
        }
    }
}*/

using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float bulletSpawnRadius = 3f;
    public float timeForDestroy = 0.5f;
    public int numBullets = 8;
    public float heightOffset = 2f; // ปรับค่านี้เพื่อยิงขึ้นหรือลงมากขึ้น

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shuriken"))
        {
            Vector3 targetPosition = other.transform.position;
            // นำ Y ของ targetPosition ขึ้นสูงขึ้น เช่น 2 เมตร
            targetPosition.y += 2.0f;
            float radius = bulletSpawnRadius;
            float angleIncrement = 360f / numBullets;

            for (int i = 0; i < numBullets; i++)
            {
                float angle = i * angleIncrement;
                float x = targetPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float z = targetPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                GameObject bullet = Instantiate(bulletPrefab, new Vector3(x, targetPosition.y, z), Quaternion.identity);

                Vector3 bulletDirection = (bullet.transform.position - targetPosition).normalized;

                // ใช้ Quaternion.LookRotation เพื่อหมุนโมเดลให้ชี้ไปทางทิศทางของแรง
                Quaternion rotation = Quaternion.LookRotation(bulletDirection);
                bullet.transform.rotation = rotation;

                bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
                Destroy(bullet, timeForDestroy);
            }

            Destroy(gameObject);
        }
    }
} 