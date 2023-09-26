/*using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyDetector : MonoBehaviour
{
    [Header("****Check Enemy****")]
    public float detectionRadius = 100f;
    public LayerMask enemyLayer;
    private List<Transform> detectedEnemies = new List<Transform>();
    public int detectedEnemyCount;
    public int enemyWasDestroyed = 0; // เพิ่มตัวแปรเพื่อเก็บจำนวนศัตรูที่ถูกทำลาย

    [Header("****Open Portal****")]
    public bool canOpenPortal = false;
    public GameObject portal;
    private PlayerController playerController;

*//*    [Header("****Show Score****")]
    
    public int runningScore;
    public Text scoreUI;
    private int freeScore = 10;
    private int langKoon;*//*

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        *//*langKoon = playerController.runsRemaining * 10;
        runningScore = langKoon + freeScore;*//*
        DetectEnemies();
        UpdateDetectedEnemyCount();
        //scoreUI.gameObject.SetActive(false);
    }

    private void Update()
    {
       *//* if (langKoon != 0)
        {
            int currentRunsRemaining = playerController.runsRemaining;
            langKoon = currentRunsRemaining * 10;
            runningScore = freeScore + langKoon;
            UpdateScore();
        }*//*
        

        DetectEnemies();
        UpdateDetectedEnemyCount();
    }



    private void DetectEnemies() //นับจำนวนศัตรู
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        detectedEnemies.Clear();
        detectedEnemyCount = 0;

        foreach (Collider collider in colliders)
        {
            detectedEnemies.Add(collider.transform);
            detectedEnemyCount++;
        }
    }


    *//*public void EnemyDestroyed(Transform enemyTransform)
    {
        if (detectedEnemies.Contains(enemyTransform))
        {
            detectedEnemies.Remove(enemyTransform);
            //EnemyCount = detectedEnemies.Count;
            
            enemyWasDestroyed++;
            UpdateDetectedEnemyCount();
        }
    }*//*
    public void EnemyDestroyed(Transform enemyTransform)
    {
        if (detectedEnemies.Contains(enemyTransform))
        {
            detectedEnemies.Remove(enemyTransform);


            UpdateDetectedEnemyCount();
        }
    }
    public void NotuseList()
    {
        int maxEnemies = detectedEnemyCount;
        if (detectedEnemyCount < maxEnemies)
        {
            enemyWasDestroyed++;
        }
    }

    void IncreaseRun()
    {
        if (enemyWasDestroyed > 0) // เพิ่ม runsRemaining ทุกครั้งที่ศัตรูถูกทำลาย
        { 
            playerController.IncreaseRunsRemaining();
        }
    }
    public void UpdateDetectedEnemyCount()
    {
        
        if (detectedEnemyCount == 0)
        {
            canOpenPortal = true;
            if (canOpenPortal == true)
            {
                portal.SetActive(true);
                //playerController.IncreaseRunsRemaining(); // เพิ่ม runsRemaining ใน PlayerController

            }
        }
        else
        {
            canOpenPortal = false;
            portal.SetActive(false);
        }
    }

    *//*    public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInZone = true;
                UpdateScore();
            }
        }*/

/* void UpdateScore()
 {
     scoreUI.text = "Score: " + runningScore;
 }*/


/*public void HandleEnemyDestroyed(Transform enemyTransform)
{
    if (enemyWasDestroyed !=0 )
    {
        playerController.IncreaseRunsRemaining();
    }
}*//*


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


}*/



/*using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("****Check Enemy****")]
    public float detectionRadius = 100f;
    public LayerMask enemyLayer;
    public int detectedEnemyCount; // No need for a list
    public int enemyWasDestroyed = 0;
    int maxEnemies;

    [Header("****Open Portal****")]
    public bool canOpenPortal = false;
    public GameObject portal;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        DetectEnemies();
        OpenPortal();
    }

    private void Update()
    {
        DetectEnemies();


        OpenPortal();
    }

    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        detectedEnemyCount = colliders.Length; // Count enemies directly
        maxEnemies = detectedEnemyCount;

        if (detectedEnemyCount != 0)
        {
            maxEnemies = detectedEnemyCount;
        }
    }

    public void EnemyDestroyed()
    {
        if (detectedEnemyCount < maxEnemies)
        {
            enemyWasDestroyed++;
        }

    }

    void IncreaseRun()
    {
        if (enemyWasDestroyed > 0) // เพิ่ม runsRemaining ทุกครั้งที่ศัตรูถูกทำลาย
        {
            playerController.IncreaseRunsRemaining();
        }
    }

*//*    void IncreaseRun()
    {
        if (detectedEnemyCount > 0)
        {
            playerController.IncreaseRunsRemaining();
        }
    }*//*

    public void OpenPortal()
    {
        if (detectedEnemyCount == 0)
        {
            canOpenPortal = true;
            if (canOpenPortal == true)
            {
                portal.SetActive(true);
            }
        }
        else
        {
            canOpenPortal = false;
            portal.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
*/

using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("****Check Enemy****")]
    public float detectionRadius = 100f;
    public LayerMask enemyLayer;
    public int detectedEnemyCount;
    public int enemyWasDestroyed = 0;
    int maxEnemies;

    [Header("****Open Portal****")]
    public bool canOpenPortal = false;
    public GameObject portal;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        DetectEnemies();
        OpenPortal();
    }

    private void Update()
    {
        DetectEnemies();
        OpenPortal();
    }

    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        detectedEnemyCount = colliders.Length; // Count enemies directly

        // Decrease runsRemaining for every decrease in detectedEnemyCount
        if (detectedEnemyCount < maxEnemies)
        {
            int decreaseAmount = maxEnemies - detectedEnemyCount;
            playerController.DecreaseRunsRemaining(decreaseAmount);
        }

        maxEnemies = detectedEnemyCount;
    }

    public void EnemyDestroyed()
    {
        if (detectedEnemyCount < maxEnemies)
        {
            enemyWasDestroyed++;
        }
    }

    public void OpenPortal()
    {
        if (detectedEnemyCount == 0)
        {
            canOpenPortal = true;
            if (canOpenPortal == true)
            {
                portal.SetActive(true);
            }
        }
        else
        {
            canOpenPortal = false;
            portal.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}