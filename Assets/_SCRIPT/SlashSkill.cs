using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class SlashSkill : MonoBehaviour
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private int slashCount;
    [Header("Skill Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float radiusOffset = 5f;
    [SerializeField] private float heightOffset = 2f;

    private SkillTurnSystem _skillSystem;
    private int cooldownTurns;
    private int currentCooldownTurns;
    private List<SlashInfo> instantiatedSlashes = new List<SlashInfo>();

    private class SlashInfo
    {
        public GameObject SlashObject;
        public Vector3 LocalPosition;
        public Rigidbody Rigidbody;
        public Collider Collider;
    }

    private void Awake()
    {
        _skillSystem = GetComponent<SkillTurnSystem>();
    }

    private void Start()
    {
        InitializeSkillSettings();
    }

    private void OnEnable()
    {
        _skillSystem.updateSkill += HandleTurnEnd;
    }

    private void OnDisable()
    {
        _skillSystem.updateSkill -= HandleTurnEnd;
    }

    private void Update()
    {
        if (currentCooldownTurns == 1)
        {
            UpdateSlashPositions();
        }
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
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitUntil(() => rb.velocity.magnitude <= 0.01f);
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
        foreach (var slashInfo in instantiatedSlashes)
        {
            LeanPool.Despawn(slashInfo.SlashObject);
        }
        instantiatedSlashes.Clear();
    }

    private void UpdateSkillSystemValues()
    {
        _skillSystem.SetCurrentValue(currentCooldownTurns > 0 ? currentCooldownTurns : cooldownTurns, 0);
    }

    private void InitSlashes()
    {
        float angleStep = 360f / slashCount;

        for (int i = 0; i < slashCount; i++)
        {
            float angle = angleStep * i;
            SlashInfo slashInfo = SpawnSlash(angle);
            instantiatedSlashes.Add(slashInfo);
        }
    }

    private SlashInfo SpawnSlash(float angle)
    {
        Vector3 position = GetSlashPosition(angle);
        GameObject slash = LeanPool.Spawn(slashPrefab, position, Quaternion.Euler(0, angle, 0));
        slash.name = "SlashSkill";
        slash.transform.SetParent(transform);
        DisableCollider(slash);

        SlashInfo slashInfo = new SlashInfo
        {
            SlashObject = slash,
            LocalPosition = slash.transform.localPosition,
            Rigidbody = slash.GetComponent<Rigidbody>(),
            Collider = slash.GetComponent<Collider>()
        };

        ResetSlashPhysics(slashInfo);
        return slashInfo;
    }

    private Vector3 GetSlashPosition(float angle)
    {
        Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radiusOffset;
        return transform.position + offset + Vector3.up * heightOffset;
    }

    private void UpdateSlashPositions()
    {
        foreach (var slashInfo in instantiatedSlashes)
        {
            slashInfo.SlashObject.transform.localPosition = slashInfo.LocalPosition;
        }
    }

    private void ResetSlashPhysics(SlashInfo slashInfo)
    {
        slashInfo.Rigidbody.velocity = Vector3.zero;
        slashInfo.Rigidbody.angularVelocity = Vector3.zero;
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
        foreach (var slashInfo in instantiatedSlashes)
        {
            if (slashInfo.SlashObject != null)
            {
                EnableCollider(slashInfo.SlashObject);
                ShootSlash(slashInfo);
            }
        }
        instantiatedSlashes.Clear();
    }

    private void ShootSlash(SlashInfo slashInfo)
    {
        slashInfo.SlashObject.transform.SetParent(null);
    
        // Calculate forward direction based on slash's orientation (assuming local forward is the desired direction)
        Vector3 forwardDirection = slashInfo.SlashObject.transform.forward;
    
        // Apply force in the forward direction
        slashInfo.Rigidbody.AddForce(forwardDirection * speed, ForceMode.Impulse);
    }
}
