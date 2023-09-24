using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openPortal : MonoBehaviour
{
    public bool canOpenPortal;
    public GameObject portal;
    public int allEnemyCount;

    private EnemyDetector enemyDetector;

    // Start is called before the first frame update
    void Start()
    {
        enemyDetector = GetComponent<EnemyDetector>();
        allEnemyCount = enemyDetector.detectedEnemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (allEnemyCount != 0)
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
}
