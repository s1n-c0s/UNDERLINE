using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class IZoneProgress : MonoBehaviour
{
    [SerializeField] private Toggle _zonePrefab;
    [SerializeField] private Toggle _jailPrefab;
    [SerializeField] private Transform toggleParent; // Add a parent for toggles in the inspector
    [SerializeField] private List<ZoneManager> zoneManagers = new List<ZoneManager>();
    [SerializeField] private List<JailObj> jailObjs = new List<JailObj>(); // List for JailObjs

    private float activeAlpha = 0.2f;
    private float inactiveAlpha = 1.0f;

    private void Awake()
    {
        InitializeZoneManagers();
        InitializeJailManagers(); // Initialize JailObjs
        InitToggles();
    }

    private void InitializeZoneManagers()
    {
        zoneManagers.AddRange(FindObjectsOfType<ZoneManager>());
        zoneManagers.Sort((z1, z2) => z2.zoneOrder.CompareTo(z1.zoneOrder)); // Sort in descending order (reverse)
    }

    private void InitializeJailManagers()
    {
        jailObjs.AddRange(FindObjectsOfType<JailObj>());
    }

    private void InitToggles()
    {
        foreach (var jailObj in jailObjs)
        {
            Toggle toggle = LeanPool.Spawn(_jailPrefab, toggleParent);
            UpdateToggle(jailObj, toggle);
            jailObj.OnJailUnlock += (jail) => UpdateToggle(jail, toggle);
        }
        
        foreach (var zoneManager in zoneManagers)
        {
            Toggle toggle = LeanPool.Spawn(_zonePrefab, toggleParent);
            UpdateToggle(zoneManager, toggle);
            zoneManager.OnZoneClear += (zone) => UpdateToggle(zone, toggle);
        }
    }

    private void UpdateToggle(ZoneManager zone, Toggle toggle)
    {
        Graphic background = toggle.targetGraphic;
        Color color = background.color;
        color.a = zone.currentState == ZoneManager.ZoneState.Cleared ? activeAlpha : inactiveAlpha;
        background.color = color;
    }

    private void UpdateToggle(JailObj jail, Toggle toggle)
    {
        Graphic background = toggle.targetGraphic;
        Color color = background.color;
        color.a = !jail.IsLocked ? activeAlpha : inactiveAlpha;
        background.color = color;
    }
}
