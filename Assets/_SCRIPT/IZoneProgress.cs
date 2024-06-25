using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class IZoneProgress : MonoBehaviour
{
    [SerializeField] private Toggle _togglePrefab;
    [SerializeField] private Transform toggleParent; // Add a parent for toggles in the inspector
    [SerializeField] private List<ZoneManager> zoneManagers = new List<ZoneManager>();

    private float activeAlpha = 0.2f;
    private float inactiveAlpha = 1.0f;

    private void Awake()
    {
        toggleParent = GetComponent<Transform>();
        InitializeZoneManagers();
        InitToggles();
    }

    private void InitializeZoneManagers()
    {
        zoneManagers.AddRange(FindObjectsOfType<ZoneManager>());
        zoneManagers.Sort((z1, z2) => z2.zoneOrder.CompareTo(z1.zoneOrder)); // Sort in descending order (reverse)
    }

    private void InitToggles()
    {
        foreach (var zoneManager in zoneManagers)
        {
            Toggle toggle = LeanPool.Spawn(_togglePrefab, toggleParent);
            UpdateToggle(zoneManager, toggle);
            zoneManager.OnZoneClear += (zone) => UpdateToggle(zone, toggle);
        }
    }

    private void UpdateToggle(ZoneManager zone, Toggle toggle)
    {
        Graphic background = toggle.targetGraphic;
        Color color = background.color;
        color.a = zone.isClear ? activeAlpha : inactiveAlpha;
        background.color = color;
    }
}