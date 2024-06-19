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
    [SerializeField] private List<GameObject> items;
    [SerializeField] private GameObject enemy;

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
        Rigidbody rb = enemy.GetComponent<Rigidbody>();

        while (rb.velocity.magnitude > 0.01f)
        {
            yield return null;
        }
        
        instantiatedBullets[currentTurnCount].SetActive(true);

        currentTurnCount++;

        if (currentTurnCount > 0 && currentTurnCount % cooldownTurns == 0)
        {
            ShootOrbs();
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
        instantiatedBullets.Clear();
        bulletAngles.Clear();

        float angleStep = 360f / items.Count;

        for (int i = 0; i < items.Count; i++)
        {
            float angle = angleStep * i;

            bulletAngles.Add(angle);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 position = transform.position + rotation * Vector3.forward * radiusOffset + Vector3.up * heightOffset;

            GameObject bullet = LeanPool.Spawn(items[i], position, rotation, transform);
            bullet.name = "Fireball";
            instantiatedBullets.Add(bullet);
            instantiatedBullets[i].SetActive(false); 
            ResetBulletPhysics(bullet);
        }
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
                bulletAngles[i] += rotationSpeed * Time.deltaTime;
                bulletAngles[i] %= 360; // Ensure the angle stays within 0-360 degrees

                float rad = Mathf.Deg2Rad * bulletAngles[i];
                Vector3 offset = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * radiusOffset;
                instantiatedBullets[i].transform.position = transform.position + offset + Vector3.up * heightOffset;
            }
        }
    }

    private void ShootOrbs()
    {
        foreach (GameObject bullet in instantiatedBullets)
        {
            if (bullet != null)
            {
                bullet.transform.SetParent(null);
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                if (bulletRigidbody != null)
                {
                    // Calculate the direction based on the bullet's current position and angle
                    float rad = Mathf.Deg2Rad * bulletAngles[instantiatedBullets.IndexOf(bullet)];
                    Vector3 direction = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
                    bulletRigidbody.AddForce(direction * power, ForceMode.Impulse);
                }
            }
            LeanPool.Despawn(bullet, 1f);
        }

        instantiatedBullets.Clear();
        bulletAngles.Clear();

        // Initialize all items again for continuous rotation
        InitAllItems();
    }
}
