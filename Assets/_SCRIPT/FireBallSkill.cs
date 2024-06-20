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

    [SerializeField] private SkillTurnSystem _skillTurnSystem;
    private int cooldownTurns;
    private int currentTurnCount;
    private List<GameObject> instantiatedBullets = new List<GameObject>();
    private List<float> bulletAngles = new List<float>();

    private void Awake()
    {   
        _skillTurnSystem = GetComponent<SkillTurnSystem>();
        Initialize();
    }

    private void Initialize()
    {
        cooldownTurns = _skillTurnSystem.CooldownTurns;
        currentTurnCount = cooldownTurns; // Start with the maximum cooldown value
        ResetSkill();
        InitAllItems();
        UpdateSkillSystemValues(); // Update skill system initially
    }

    private void OnEnable()
    {
        _skillTurnSystem.OnTurnEnd += HandleTurnEnd;
    }

    private void OnDisable()
    {
        _skillTurnSystem.OnTurnEnd -= HandleTurnEnd;
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

        ActivateBullet(currentTurnCount - 1); // Activate bullet corresponding to currentTurnCount
        currentTurnCount--;
        UpdateSkillSystemValues();

        if (currentTurnCount <= 0)
        {
            ShootOrbs();
            ResetSkill();
            currentTurnCount = cooldownTurns; // Reset currentTurnCount to cooldownTurns
            InitAllItems();
        }
    }

    private void ActivateBullet(int index)
    {
        if (index >= 0 && index < instantiatedBullets.Count)
        {
            instantiatedBullets[index].SetActive(true);
        }
    }

    private void ResetSkill()
    {
        foreach (GameObject bullet in instantiatedBullets)
        {
            LeanPool.Despawn(bullet); // Despawn bullets from the pool
        }
        instantiatedBullets.Clear();
        bulletAngles.Clear();
    }

    private void UpdateSkillSystemValues()
    {
        int displayedValue = currentTurnCount == 0 ? cooldownTurns : currentTurnCount;
        _skillTurnSystem.SetCurrentValue(displayedValue, 0);
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
        bullet.transform.parent = transform; // Simplified setting parent to transform
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
                ShootBullet(bullet);
            }
        }

        instantiatedBullets.Clear();
        bulletAngles.Clear();
    }

    private void ShootBullet(GameObject bullet)
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
        }
    }
}
