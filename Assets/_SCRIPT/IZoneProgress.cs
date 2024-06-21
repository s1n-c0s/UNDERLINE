using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IZoneProgress : MonoBehaviour
{
    [SerializeField] private int currentProgress = 0;
    [SerializeField] private Slider _zoneBar;
    [SerializeField] private List<ZoneManager> zoneManager;
    
    private void Awake()
    {
        _zoneBar = GetComponent<Slider>(); // Ensure _zoneBar is correctly linked in the Inspector

        zoneManager.AddRange(FindObjectsOfType<ZoneManager>());
        zoneManager.Sort((z1, z2) => z1.zoneOrder.CompareTo(z2.zoneOrder));
        _zoneBar.value = currentProgress;
        _zoneBar.maxValue = zoneManager.Count;
        // Add listener to slider value change
        _zoneBar.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnEnable()
    {
        if (zoneManager != null)
        { 
            foreach (ZoneManager zone in zoneManager)
            {
                zone.OnZoneClear += HandleZoneClear;
            }
        }
    }

    private void OnDisable()
    {
        if (zoneManager != null)
        { 
            foreach (ZoneManager zone in zoneManager)
            {
                zone.OnZoneClear -= HandleZoneClear;
            }
        }
    }

    private void HandleZoneClear(ZoneManager zoneManager)
    {
        if (_zoneBar.value < _zoneBar.maxValue)
        {
            _zoneBar.value++;
            currentProgress = (int)_zoneBar.value;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        currentProgress = (int)value;
        _zoneBar.value = currentProgress;
    }
}
