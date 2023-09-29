using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnemyDetector : MonoBehaviour
{
    [Header("****Check Enemy****")]
    public float detectionRadius = 100f;
    public LayerMask enemyLayer;
    public int detectedEnemyCount;
    //public int enemyWasDestroyed = 0;
    //int maxEnemies;

    private HashSet<Transform> detectedEnemies = new HashSet<Transform>(); // เปลี่ยน List เป็น HashSet

    [Header("****Open Portal****")]
    public bool canOpenPortal = false;
    public GameObject portal;
    private PlayerController playerController;

    [Header("****UI****")]
    public TextMeshProUGUI detectedEnemyText;
    public TextMeshProUGUI potalText;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        DetectEnemies();
        OpenPortal();
        UpdateDetectedEnemyText();
    }

    private void Update()
    {
        DetectEnemies();
        OpenPortal();
        UpdateDetectedEnemyText();
    }

    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        // ล้าง detectedEnemies และ detectedEnemyCount ทุกครั้งที่ตรวจพบใหม่
        detectedEnemies.Clear();
        detectedEnemyCount = 0;

        foreach (Collider collider in colliders)
        {
            Transform enemyTransform = collider.transform;

            // ตรวจสอบว่าเคยเพิ่ม enemyTransform ลงใน HashSet แล้วหรือยัง
            if (!detectedEnemies.Contains(enemyTransform))
            {
                detectedEnemies.Add(enemyTransform);
                detectedEnemyCount++;
            }
        }

       /* // ลด runsRemaining สำหรับการลด detectedEnemyCount
        if (detectedEnemyCount < maxEnemies)
        {
            int decreaseAmount = maxEnemies - detectedEnemyCount;
            playerController.DecreaseRunsRemaining(decreaseAmount);
        }

        maxEnemies = detectedEnemyCount;*/
    }

   /* public void EnemyDestroyed()
    {
        if (detectedEnemyCount < maxEnemies)
        {
            enemyWasDestroyed++;
        }
    }*/

    public void OpenPortal()
    {
        if (detectedEnemyCount == 0)
        {
            canOpenPortal = true;
            if (canOpenPortal == true)
            {
                portal.SetActive(true);
                potalText.text = "Potal Open";
            }
        }
        else
        {
            canOpenPortal = false;
            portal.SetActive(false);
            potalText.text = "Potal Close";
        }
    }

    private void UpdateDetectedEnemyText()
    {
        // ตั้งค่าข้อความ UI Text ให้แสดง detectedEnemyCount
        if (detectedEnemyText != null)
        {
            detectedEnemyText.text = "Detected Enemies: " + detectedEnemyCount;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

