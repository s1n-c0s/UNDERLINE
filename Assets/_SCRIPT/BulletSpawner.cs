/*using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // ออบเจ็กต์กระสุนที่จะยิง
    public float bulletSpeed = 5f; // ความเร็วของกระสุน

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่า game object ที่ชนมี tag "BombEnemy" หรือไม่
        if (other.CompareTag("BombEnemy"))
        {
            // ทำการยิงกระสุน 8 ลูกทันที
            for (int i = 0; i < 2; i++)
            {
                // สร้างออบเจ็กต์กระสุน
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // หมุนออบเจ็กต์กระสุนเพื่อให้กระสุนถูกยิงไปทุกทิศทาง
                float angle = i * 45f;
                bullet.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // กำหนดความเร็วให้กับกระสุน
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            }

            // ทำลาย game object ที่ชน
            Destroy(other.gameObject);
        }
    }
}*/