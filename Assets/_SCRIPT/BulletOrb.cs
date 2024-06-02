using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class BulletOrb : MonoBehaviour
{   
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float power = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private List<GameObject> items;

    private List<GameObject> instantiatedBullets = new List<GameObject>();

    private void Start()
    {
        InitItems();
        StartCoroutine(RotateOrb());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootOrb();
        }
    }

    private void InitItems()
    {
        int numberOfItems = items.Count;
        float angleIncrement = 360f / numberOfItems;

        for (int i = 0; i < numberOfItems; i++)
        {
            float angle = angleIncrement * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 position = transform.position + rotation * Vector3.forward * radiusOffset + Vector3.up * heightOffset;

            GameObject bullet = LeanPool.Spawn(items[i], position, rotation, transform);
            bullet.name = "Kunai";
            instantiatedBullets.Add(bullet);
        }
    }

    private IEnumerator RotateOrb()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ShootOrb()
    {
        foreach (GameObject bullet in instantiatedBullets)
        {
            bullet.transform.SetParent(null);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                Vector3 direction = bullet.transform.forward;
                bulletRigidbody.AddForce(direction * power, ForceMode.Impulse);
            }
        }

        instantiatedBullets.Clear();
    }
}