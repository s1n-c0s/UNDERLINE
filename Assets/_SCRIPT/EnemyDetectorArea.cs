using System;
using TMPro;
using UnityEngine;

public class EnemyDetectorArea : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    private BoxCollider boxCollider;
    public int enemyCount; // This will keep track of the number of enemies in the detection area

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        //enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        enemyCountText.text = "Enemies: " + enemyCount; // Display initial count
    }

    private void Update()
    {
        enemyCountText.text = "Enemies: " + enemyCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyCount++;
            //Debug.Log("Enemy entered detection area. Total enemies: " + enemyCount);
        }
        
        /*if (other.CompareTag("Enemy")) 
        {
            enemyCount++;
            Debug.Log("Enemy entered detection area. Total enemies: " + enemyCount);
        }*/
    }
    
    public void DecreseEnemy()
    {
        if (enemyCount > 0)
        {
            enemyCount--;
        }
        /*enemyCountText.text = "Enemies: " + enemyCount;*/
    }
    
    public int GetCurrentEnemy()
    {
        return enemyCount;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            enemyCount--;
            Debug.Log("Enemy left detection area. Total enemies: " + enemyCount);
        }
    }*/
}