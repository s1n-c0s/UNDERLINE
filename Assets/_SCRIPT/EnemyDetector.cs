using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyDetector : MonoBehaviour
{
    [Header("****Check Enemy****")]
    public float detectionRadius = 100f;
    public LayerMask enemyLayer;
    private int EnemyCount;
    private List<Transform> detectedEnemies = new List<Transform>();
    public int detectedEnemyCount;
    private int enemyWasDestroyed = 0; // เพิ่มตัวแปรเพื่อเก็บจำนวนศัตรูที่ถูกทำลาย

    [Header("****Open Portal****")]
    public bool canOpenPortal = false;
    public GameObject portal;
    

    [Header("****Show Score****")]
    public PlayerController playerController;
    public int runningScore;
    public Text scoreUI;
    private int freeScore = 10;
    private int langKoon;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        langKoon = playerController.runsRemaining * 10;
        runningScore = langKoon + freeScore;
        DetectEnemies();
        UpdateDetectedEnemyCount();
        //scoreUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (langKoon != 0)
        {
            int currentRunsRemaining = playerController.runsRemaining;
            langKoon = currentRunsRemaining * 10;
            runningScore = freeScore + langKoon;
            UpdateScore();
        }
        

        DetectEnemies();
        UpdateDetectedEnemyCount();
    }



    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        detectedEnemies.Clear();
        EnemyCount = 0;

        foreach (Collider collider in colliders)
        {
            detectedEnemies.Add(collider.transform);
            EnemyCount++;
        }
    }

    public void UpdateDetectedEnemyCount()
    {
        detectedEnemyCount = EnemyCount / 2;
        if (detectedEnemyCount == 0)
        {
            canOpenPortal = true;
            if (canOpenPortal == true)  // Use == for comparison, not =
            {
                portal.SetActive(true); // Activate the portal GameObject
            }
        }
        else
        {
            canOpenPortal = false;
            portal.SetActive(false); // Deactivate the portal GameObject
        }
    }

/*    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = true;
            UpdateScore();
        }
    }*/

    void UpdateScore()
    {
        scoreUI.text = "Score: " + runningScore;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        foreach (Transform enemyTransform in detectedEnemies)
        {
            Gizmos.DrawLine(transform.position, enemyTransform.position);
        }
    }

    public void EnemyDestroyed(Transform enemyTransform)
    {
        if (detectedEnemies.Contains(enemyTransform))
        {
            detectedEnemies.Remove(enemyTransform);
            EnemyCount = detectedEnemies.Count;
            UpdateDetectedEnemyCount();
            enemyWasDestroyed++;
        }
    }
}