using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class BulletOrb : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float power = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private AnimationCurve speedCurve; // Animation curve for speed adjustment
    [SerializeField] private List<GameObject> items;

    [Header("VFX")]
    [SerializeField] private GameObject fx_Shoot;

    private bool canShoot = true;
    private float timer;
    private List<GameObject> instantiatedBullets = new List<GameObject>();

    private void Start()
    {
        InitItems();
    }

    private void Update()
    {
        RotateOrb();
        if (!canShoot)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                InitItems();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canShoot)
        {
            ShootOrb();
            Vector3 spawnPosition = transform.position + Vector3.up * 3.5f;
            GameObject fx_shoot = LeanPool.Spawn(fx_Shoot, spawnPosition, Quaternion.identity);
            LeanPool.Despawn(fx_shoot, 3f);
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

            BoxCollider bulletCollider = bullet.GetComponent<BoxCollider>();
            bulletCollider.enabled = false;
            
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = Vector3.zero;
                bulletRigidbody.angularVelocity = Vector3.zero;
            }
        }

        canShoot = true;
    }

    private void RotateOrb()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void ShootOrb()
    {
        foreach (GameObject bullet in instantiatedBullets)
        {
            bullet.transform.SetParent(null);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                StartCoroutine(AdjustBulletSpeed(bulletRigidbody));
            }
            
            BoxCollider bulletCollider = bullet.GetComponent<BoxCollider>();
            bulletCollider.enabled = true;
        }

        instantiatedBullets.Clear();
        canShoot = false;
        timer = cooldown;
    }

    private IEnumerator AdjustBulletSpeed(Rigidbody bulletRigidbody)
    {
        float elapsedTime = 0f;
        Vector3 initialDirection = bulletRigidbody.transform.forward;

        while (elapsedTime <= 1f) // Ensure the curve evaluates from 0 to 1
        {
            float curveValue = speedCurve.Evaluate(elapsedTime);
            bulletRigidbody.velocity = initialDirection * curveValue * power;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
