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
    private List<Vector3> localSlashPositions = new List<Vector3>();

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

    private void Update()
    {
        UpdateSlashPositions();
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
        localSlashPositions.Clear();
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
            GameObject slash = SpawnSlash(angle);
            instantiatedSlashes.Add(slash);
            localSlashPositions.Add(slash.transform.localPosition);
        }
    }

    private GameObject SpawnSlash(float angle)
    {
        Vector3 position = GetSlashPosition(angle);
        GameObject slash = LeanPool.Spawn(slashPrefab, position, Quaternion.Euler(0, angle, 0));
        slash.name = "SlashSkill";
        slash.transform.SetParent(transform);
        DisableCollider(slash);
        ResetSlashPhysics(slash);
        return slash;
    }

    private Vector3 GetSlashPosition(float angle)
    {
        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radiusOffset;
        return transform.position + offset + Vector3.up * heightOffset;
    }

    private void UpdateSlashPositions()
    {
        for (int i = 0; i < instantiatedSlashes.Count; i++)
        {
            instantiatedSlashes[i].transform.localPosition = localSlashPositions[i];
        }
    }

    private void ResetSlashPhysics(GameObject slash)
    {
        if (slash.TryGetComponent(out Rigidbody slashRigidbody))
        {
            slashRigidbody.velocity = Vector3.zero;
            slashRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void DisableCollider(GameObject slash)
    {
        if (slash.TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
    }

    private void EnableCollider(GameObject slash)
    {
        if (slash.TryGetComponent(out Collider collider))
        {
            collider.enabled = true;
        }
    }

    private void ShootSlashes()
    {
        foreach (GameObject slash in instantiatedSlashes)
        {
            if (slash != null)
            {
                EnableCollider(slash);
                ShootSlash(slash);
            }
        }
        instantiatedSlashes.Clear();
        localSlashPositions.Clear();
    }

    private void ShootSlash(GameObject slash)
    {
        slash.transform.SetParent(null);
        if (slash.TryGetComponent(out Rigidbody slashRigidbody))
        {
            Vector3 direction = (slash.transform.position - transform.position).normalized;
            slashRigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}
