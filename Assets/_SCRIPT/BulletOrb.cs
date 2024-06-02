using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOrb : MonoBehaviour
{   
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 0f; // Adjust this if needed for height
    [SerializeField] private List<GameObject> items;

    private void Start()
    {
        InitItems();
        RotateOrb();
    }

    private void InitItems()
    {
        int numberOfItems = items.Count;
        float angleIncrement = 360f / numberOfItems;

        for (int i = 0; i < numberOfItems; i++)
        {
            float angle = angleIncrement * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = transform.position + direction * radiusOffset + Vector3.up * heightOffset;

            Instantiate(items[i], position, rotation, transform);
        }
    }

    private void RotateOrb()
    {
        RotateOnce();
    }

    private void RotateOnce()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        Invoke(nameof(RotateOnce), Time.deltaTime);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
        CancelInvoke(nameof(RotateOnce));
    }
}