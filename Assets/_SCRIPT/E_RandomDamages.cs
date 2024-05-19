using UnityEngine;
using System.Collections.Generic;

public class E_RandomDamages : MonoBehaviour
{
    [SerializeField] private List<int> randomDamages;
    private List<int> usedDamages = new List<int>();

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public int GetUnusedRandomDamage()
    {
        // Shuffle the list before selecting a random damage
        ShuffleList(randomDamages);
        int damage = randomDamages[0];
        // Move the used damage to the usedDamages list
        randomDamages.RemoveAt(0);
        usedDamages.Add(damage);
        // Check if we have used all the damages
        if (randomDamages.Count == 0)
        {
            // If so, shuffle the used damages back into the randomDamages list
            randomDamages.AddRange(usedDamages);
            usedDamages.Clear();
        }
        return damage;
    }
}