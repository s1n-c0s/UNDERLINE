using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TextMeshPro namespace

public class GameSettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown screenModeDropdown;
    public Slider musicVolumeSlider;

    private Resolution[] resolutions;

    private void Start()
    {
        // Populate the resolution dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (var resolution in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(resolution.width + "x" + resolution.height);
            resolutionDropdown.options.Add(option);
        }

        resolutionDropdown.RefreshShownValue();

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
        Resolution selectedResolution = resolutions[index];
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