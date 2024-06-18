using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class FireBallSkill : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float power = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private List<GameObject> items;

    private int cooldownTurns;
    private int currentTurnCount;
    private List<GameObject> instantiatedBullets = new List<GameObject>();

    private SkillTurnSystem skillSystem;

    private void Start()
    {
        skillSystem = GetComponent<SkillTurnSystem>();
        InitializeSkillSettings();
    }

    private void OnEnable()
    {
        SkillTurnSystem.OnTurnEnd += HandleTurnEnd;
    }

    private void OnDisable()
    {
        SkillTurnSystem.OnTurnEnd -= HandleTurnEnd;
    }

    private void InitializeSkillSettings()
    {
        cooldownTurns = skillSystem.CooldownTurns;
        ResetSkill();
    }

    private void HandleTurnEnd()
    {
        InitNextItem();

        currentTurnCount++;

        if (currentTurnCount > 0 && currentTurnCount % cooldownTurns == 0)
        {
            ShootOrb();
        }
    }

    private void ResetSkill()
    {
        currentTurnCount = 0;
        UpdateSkillSystemValues();
    }

    private void UpdateSkillSystemValues()
    {
        skillSystem.SetCurrentValue(0, currentTurnCount); // Assuming SetCurrentValue expects cooldown = 0 when not on cooldown
    }

    private void InitNextItem()
    {
        int itemIndex = currentTurnCount % items.Count;
        float angleIncrement = 360f / items.Count;
        float angle = angleIncrement * itemIndex;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 position = transform.position + rotation * Vector3.forward * radiusOffset + Vector3.up * heightOffset;

        GameObject bullet = LeanPool.Spawn(items[itemIndex], position, rotation, transform);
        bullet.name = "Fireball";
        instantiatedBullets.Add(bullet);

        ResetBulletPhysics(bullet);
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

    private void Update()
    {
        RotateOrb();
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
                bulletRigidbody.AddForce(bullet.transform.forward * power, ForceMode.Impulse);
            }
        }

        instantiatedBullets.Clear();
    }
}
