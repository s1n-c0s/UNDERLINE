using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject itemPrefab;
        public float dropChance; // Used as a weight for the likelihood of spawning.
    }

    public List<LootItem> lootTable = new List<LootItem>();
    public float spawnRadius = 5f;
    public Vector2 forceRange = new Vector2(1000, 1500);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnItemsBasedOnChance();
        }
    }

    void SpawnItemsBasedOnChance()
    {
        for (int i = 0; i < lootTable.Count; i++)
        {
            LootItem selectedLootItem = SelectItemBasedOnChance();
            if (selectedLootItem != null)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();
                GameObject spawnedItem = Instantiate(selectedLootItem.itemPrefab, spawnPos, Quaternion.identity);
                AddForceToItem(spawnedItem);
            }
        }
    }

    LootItem SelectItemBasedOnChance()
    {
        float totalWeight = 0f;
        foreach (var lootItem in lootTable)
        {
            totalWeight += lootItem.dropChance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var lootItem in lootTable)
        {
            currentWeight += lootItem.dropChance;
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
