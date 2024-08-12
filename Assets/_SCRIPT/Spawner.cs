using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject itemPrefab;
        [Range(0, 100)] // This attribute ensures dropChance is between 0 and 100
        public float dropChancePercent; // Represents drop chance in percentage
    }

    public List<LootItem> lootTable = new List<LootItem>();
    public float spawnRadius = 5f;
    public Vector2 forceRange = new Vector2(1000, 1500);

    public void SpawnItemsBasedOnChance()
    {
        for (int i = 0; i < lootTable.Count; i++)
        {
            LootItem selectedLootItem = SelectItemBasedOnChance();
            if (selectedLootItem != null)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();
                GameObject spawnedItem = LeanPool.Spawn(selectedLootItem.itemPrefab, spawnPos, Quaternion.identity);
                AddForceToItem(spawnedItem);
            }
        }
    }

    LootItem SelectItemBasedOnChance()
    {
        float totalWeight = 0f;
        foreach (var lootItem in lootTable)
        {
            totalWeight += lootItem.dropChancePercent; // Adjusted to use dropChancePercent
        }

        float randomValue = Random.Range(0, 100); // Adjusted to generate random value between 0 and 100
        float currentWeight = 0f;

        foreach (var lootItem in lootTable)
        {
            currentWeight += lootItem.dropChancePercent;
            if (randomValue <= currentWeight)
            {
                return lootItem;
            }
        }

        return null; // In case something goes wrong.
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Keep it on the same vertical level, adjust if needed
        return transform.position + randomDirection;
    }

    void AddForceToItem(GameObject item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = (Vector3.up + Random.onUnitSphere).normalized;
            float forceMagnitude = Random.Range(forceRange.x, forceRange.y);
            rb.AddForce(forceDirection * forceMagnitude);
        }
    }
}
