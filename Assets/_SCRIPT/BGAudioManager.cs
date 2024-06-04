using UnityEngine;
using UnityEngine.SceneManagement;

public class BGAudioManager : MonoBehaviour
{
    public static BGAudioManager Instance;

    public AudioSource audioSource;
    public AudioClip[] zone;
    private int currentZoneIndex = -1;
    private AudioClip previousClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayLevelMusic(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int levelIndex = scene.buildIndex;
        PlayLevelMusic(levelIndex);

        if (IsMainMenuScene(levelIndex))
        {
            Destroy(gameObject);
        }
    }

    void PlayLevelMusic(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            int zoneIndex = GetZoneIndexForLevel(levelIndex);

            if (zoneIndex != currentZoneIndex)
            {
                AudioClip musicToPlay = zone[zoneIndex];

                if (musicToPlay == null && previousClip != null)
                {
                    audioSource.clip = previousClip;
                }
                else
                {
                    audioSource.clip = musicToPlay;
                }

                audioSource.Play();
                currentZoneIndex = zoneIndex;
                previousClip = musicToPlay;
            }
        }
        else
        {
            Debug.LogError("Invalid level index: " + levelIndex);
        }
    }

    int GetZoneIndexForLevel(int levelIndex)
    {
        if (levelIndex >= 1 && levelIndex <= 3)
        {
            return 0;
        }
        else if (levelIndex >= 4 && levelIndex <= 6)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    bool IsMainMenuScene(int levelIndex)
    {
        return levelIndex == 0;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ToggleMute(bool mute)
    {
        audioSource.mute = mute;
    }
}
