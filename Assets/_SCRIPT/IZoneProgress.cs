using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneProgress : MonoBehaviour
{
    [SerializeField] private int currentProgress = 0;
    [SerializeField] private Slider zoneBar;
    [SerializeField] private List<ZoneManager> zoneManagers = new List<ZoneManager>();

    private void Awake()
    {
        zoneBar = GetComponent<Slider>(); // Ensure zoneBar is correctly linked in the Inspector

        InitializeZoneManagers();
        InitializeSlider();
    }

    private void InitializeZoneManagers()
    {
        zoneManagers.AddRange(FindObjectsOfType<ZoneManager>());
        zoneManagers.Sort((z1, z2) => z1.zoneOrder.CompareTo(z2.zoneOrder));
    }

    private void InitializeSlider()
    {
        zoneBar.value = currentProgress;
        zoneBar.maxValue = zoneManagers.Count;
        zoneBar.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnEnable()
    {
        foreach (var zoneManager in zoneManagers)
        {
            zoneManager.OnZoneClear += HandleZoneClear;
        }
    }

    private void OnDisable()
    {
        foreach (var zoneManager in zoneManagers)
        {
            zoneManager.OnZoneClear -= HandleZoneClear;
        }
    }

    private void HandleZoneClear(ZoneManager zoneManager)
    {
        if (currentProgress < zoneBar.maxValue)
        {
            currentProgress++;
            zoneBar.value = currentProgress;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        currentProgress = Mathf.RoundToInt(value);
        zoneBar.value = currentProgress;
    }
}