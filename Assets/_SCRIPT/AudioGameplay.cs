using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] zone;

    private int currentZoneIndex = -1;
    private AudioClip previousClip;
    private bool isMuted;
    private const string MutePrefKey = "MuteAudio";

    public bool IsMuted => isMuted; // Public getter for the mute state

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMutePreference();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource ??= GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayLevelMusic(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayLevelMusic(scene.buildIndex);
        if (IsMainMenuScene(scene.buildIndex)) Destroy(gameObject);
    }

    void PlayLevelMusic(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        int zoneIndex = GetZoneIndexForLevel(levelIndex);
        if (zoneIndex == currentZoneIndex) return;

        AudioClip musicToPlay = zone[zoneIndex];
        audioSource.clip = musicToPlay ?? previousClip;
        audioSource.Play();
        currentZoneIndex = zoneIndex;
        previousClip = musicToPlay;
        audioSource.mute = isMuted;
    }

    int GetZoneIndexForLevel(int levelIndex) =>
        levelIndex switch
        {
            >= 1 and <= 3 => 0,
            >= 4 and <= 6 => 1,
            _ => 0,
        };

    bool IsMainMenuScene(int levelIndex) => levelIndex == 0;

    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        SaveMutePreference();
    }

    void SaveMutePreference()
    {
        PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadMutePreference()
    {
        isMuted = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;
    }
}
