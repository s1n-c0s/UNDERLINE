using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class EnemyDetectorArea : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    private BoxCollider boxCollider;
    private HashSet<Collider> detectedEnemies = new HashSet<Collider>();

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
            detectedEnemies.Add(other);
            UpdateEnemyCountText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && detectedEnemies.Contains(other))
        {
            detectedEnemies.Remove(other);
            UpdateEnemyCountText();
        }
    }

    public void DecreaseEnemy()
    {
        if (detectedEnemies.Count > 0)
        {
            var firstEnemy = detectedEnemies.First();
            detectedEnemies.Remove(firstEnemy);
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
}