using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class SlashSkill : MonoBehaviour
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private int slash;

    [Header("Skill Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 2f;

    private SkillTurnSystem _skillSystem;
    private int cooldownTurns;
    private int currentCooldownTurns;
    private List<GameObject> instantiatedSlashes = new List<GameObject>();
    private List<float> slashAngles = new List<float>();

    private void Start()
    {
        _skillSystem = GetComponent<SkillTurnSystem>();
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
        cooldownTurns = _skillSystem.CooldownTurns;
        ResetCooldown();
        UpdateSkillSystemValues();
    }

    private void ResetCooldown()
    {
        currentCooldownTurns = cooldownTurns;
    }

    private void HandleTurnEnd()
    {
        StartCoroutine(WaitForVelocityZeroAndInit());
    }

    private IEnumerator WaitForVelocityZeroAndInit()
    {
        yield return new WaitUntil(() => GetComponent<Rigidbody>().velocity.magnitude <= 0.01f);
        currentCooldownTurns--;
        UpdateSkillSystemValues();

        if (currentCooldownTurns == 1)
        {
            InitSlashes();
        }
        else if (currentCooldownTurns <= 0)
        {
            ShootSlashes();
            ResetSkill();
            ResetCooldown();
            UpdateSkillSystemValues();
        }
    }

    private void ResetSkill()
    {
        foreach (var slash in instantiatedSlashes)
        {
            LeanPool.Despawn(slash);
        }
        instantiatedSlashes.Clear();
        slashAngles.Clear();
    }

    private void UpdateSkillSystemValues()
    {
        _skillSystem.SetCurrentValue(currentCooldownTurns > 0 ? currentCooldownTurns : cooldownTurns, 0);
    }

    private void InitSlashes()
    {
        float angleStep = 360f / slash;

        for (int i = 0; i < slash; i++)
        {
            float angle = angleStep * i;
            slashAngles.Add(angle);
            instantiatedSlashes.Add(SpawnSlash(angle));
        }
    }

    private GameObject SpawnSlash(float angle)
    {
        Vector3 position = GetSlashPosition(angle);
        GameObject slash = LeanPool.Spawn(slashPrefab, position, Quaternion.Euler(0, angle, 0));
        slash.name = "Slash";
        slash.transform.SetParent(transform);
        ResetSlashPhysics(slash);
        return slash;
    }

    private Vector3 GetSlashPosition(float angle)
    {
        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radiusOffset;
        return transform.position + offset + Vector3.up * heightOffset;
    }

    private void ResetSlashPhysics(GameObject slash)
    {
        if (slash.TryGetComponent(out Rigidbody slashRigidbody))
        {
            slashRigidbody.velocity = Vector3.zero;
            slashRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void ShootSlashes()
    {
        foreach (GameObject slash in instantiatedSlashes)
        {
            if (slash != null)
            {
                ShootSlash(slash);
            }
        }
        instantiatedSlashes.Clear();
        slashAngles.Clear();
    }

    private void ShootSlash(GameObject slash)
    {
        slash.transform.SetParent(null);
        if (slash.TryGetComponent(out Rigidbody slashRigidbody))
        {
            int index = instantiatedSlashes.IndexOf(slash);
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * slashAngles[index]), 0, Mathf.Cos(Mathf.Deg2Rad * slashAngles[index]));
            slashRigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
