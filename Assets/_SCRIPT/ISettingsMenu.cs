using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ISettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown screenModeDropdown;
    public Slider musicVolumeSlider;

    private List<Resolution> validResolutions;

    private void Start()
    {
        // Get a list of valid resolutions for the screen
        validResolutions = new List<Resolution>();
        Resolution currentResolution = Screen.currentResolution;

        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.width >= 1280 && resolution.height >= 720 && resolution.refreshRate == currentResolution.refreshRate)
            {
                validResolutions.Add(resolution);
            }
        }

        // Populate the resolution dropdown with valid resolutions
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < validResolutions.Count; i++)
        {
            Resolution resolution = validResolutions[i];
            string resolutionOption = resolution.width + "x" + resolution.height;

            if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
            {
                currentResolutionIndex = i;
            }

            resolutionOptions.Add(resolutionOption);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Populate the screen mode dropdown
        screenModeDropdown.ClearOptions();
        List<string> screenModeOptions = new List<string>
        {
            "Fullscreen",
            "Windowed"
        };

        screenModeDropdown.AddOptions(screenModeOptions);
        screenModeDropdown.value = Screen.fullScreen ? 0 : 1;
        screenModeDropdown.RefreshShownValue();

        // Add listener for resolution change
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        // Add listener for screen mode change
        screenModeDropdown.onValueChanged.AddListener(ChangeScreenMode);

        // Add listener for music volume change
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    private void ChangeResolution(int index)
    {
        // Change the game's resolution
        Resolution selectedResolution = validResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    private void ChangeScreenMode(int index)
    {
        // Change the screen mode (fullscreen or windowed)
        bool isFullScreen = (index == 0) ? true : false;
        Screen.fullScreen = isFullScreen;
    }

    private void ChangeMusicVolume(float volume)
    {
        // Adjust the music volume (you can link this to your audio system)
        // Example: AudioManager.SetMusicVolume(volume);
    }
}
