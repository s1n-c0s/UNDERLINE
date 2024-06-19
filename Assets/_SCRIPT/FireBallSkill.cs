using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class FireBallSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float power = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private GameObject fireballPrefab;

    private SkillTurnSystem skillSystem;
    private int cooldownTurns;
    private int currentTurnCount;
    private List<GameObject> instantiatedBullets = new List<GameObject>();
    private List<float> bulletAngles = new List<float>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        skillSystem = GetComponent<SkillTurnSystem>();
        cooldownTurns = skillSystem.CooldownTurns;
        ResetSkill();
        InitAllItems();
    }

    private void OnEnable()
    {
        SkillTurnSystem.OnTurnEnd += HandleTurnEnd;
    }

    private void OnDisable()
    {
        SkillTurnSystem.OnTurnEnd -= HandleTurnEnd;
    }

    private void HandleTurnEnd()
    {
        StartCoroutine(WaitForVelocityZeroAndInit());
    }

    private IEnumerator WaitForVelocityZeroAndInit()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        while (rb.velocity.magnitude > 0.01f)
        {
            yield return null;
        }

        ActivateBullet(currentTurnCount);
        currentTurnCount++;

        if (currentTurnCount > 0 && currentTurnCount % cooldownTurns == 0)
        {
            ShootOrbs();
            ResetSkill();
            InitAllItems();
        }
    }

    private void ActivateBullet(int index)
    {
        if (index < instantiatedBullets.Count)
        {
            instantiatedBullets[index].SetActive(true);
        }
    }

    private void ResetSkill()
    {
        currentTurnCount = 0;
        instantiatedBullets.Clear();
        bulletAngles.Clear();
        UpdateSkillSystemValues();
    }

    private void UpdateSkillSystemValues()
    {
        skillSystem.SetCurrentValue(0, currentTurnCount);
    }

    private void InitAllItems()
    {
        float angleStep = 360f / cooldownTurns;

        for (int i = 0; i < cooldownTurns; i++)
        {
            float angle = angleStep * i;
            bulletAngles.Add(angle);

            GameObject bullet = SpawnBullet(angle);
            instantiatedBullets.Add(bullet);
            ResetBulletPhysics(bullet);
        }
    }

    private GameObject SpawnBullet(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radiusOffset;
        Vector3 position = transform.position + offset + Vector3.up * heightOffset;

        GameObject bullet = LeanPool.Spawn(fireballPrefab, position, rotation);
        bullet.name = "Fireball";
        bullet.transform.parent = this.transform;
        bullet.SetActive(false);
        return bullet;
    }

    private void ResetBulletPhysics(GameObject bullet)
    {
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        RotateOrbs();
    }

    private void RotateOrbs()
    {
        for (int i = 0; i < instantiatedBullets.Count; i++)
        {
            if (instantiatedBullets[i] != null)
            {
                UpdateBulletPosition(i);
            }
        }
    }

    private void UpdateBulletPosition(int index)
    {
        bulletAngles[index] += rotationSpeed * Time.deltaTime;
        bulletAngles[index] %= 360;

        float rad = Mathf.Deg2Rad * bulletAngles[index];
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radiusOffset;
        instantiatedBullets[index].transform.position = transform.position + offset + Vector3.up * heightOffset;
    }

    private void ShootOrbs()
    {
        foreach (GameObject bullet in instantiatedBullets)
        {
            if (bullet != null)
            {
                StartCoroutine(ShootAndDespawnBullet(bullet));
            }
        }

        instantiatedBullets.Clear();
        bulletAngles.Clear();
    }

    private IEnumerator ShootAndDespawnBullet(GameObject bullet)
    {
        bullet.transform.SetParent(null);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            int index = instantiatedBullets.IndexOf(bullet);
            float angle = bulletAngles[index];
            float rad = Mathf.Deg2Rad * angle;
            Vector3 direction = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));

            bulletRigidbody.AddForce(direction * power, ForceMode.Impulse);
            Debug.Log($"Bullet {bullet.name} shot in direction {direction} with power {power}");
        }

        yield return new WaitForSeconds(1f);
        LeanPool.Despawn(bullet);
    }
}
