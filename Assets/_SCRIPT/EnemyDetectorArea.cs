using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class EnemyDetectorArea : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    private BoxCollider boxCollider;
    private List<GameObject> detectedEnemies = new List<GameObject>();

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        UpdateEnemyCountText(); // Display initial count
    }

    private void Update()
    {
        UpdateEnemyCountText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            detectedEnemies.Add(other.gameObject);
            UpdateEnemyCountText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && detectedEnemies.Contains(other.gameObject))
        {
            detectedEnemies.Remove(other.gameObject);
            UpdateEnemyCountText();
        }
    }

    public void DecreaseEnemy(GameObject enemy)
    {
        if (detectedEnemies.Contains(enemy))
        {
            detectedEnemies.Remove(enemy);
            UpdateEnemyCountText();
        }
    }

    private void UpdateEnemyCountText()
    {
        enemyCountText.text = "Enemies: " + detectedEnemies.Count;
    }

    public int GetCurrentEnemy()
    {
        return detectedEnemies.Count;
    }

    public GameObject[] GetDetectedEnemiesArray()
    {
        return detectedEnemies.ToArray();
    }
}