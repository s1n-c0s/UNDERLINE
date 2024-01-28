using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] zone;
    private int currentZoneIndex = -1;
    private AudioClip previousClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

        // Check if the main menu scene is loaded
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
                    // Play the previous valid AudioClip
                    audioSource.clip = previousClip;
                }
                else
                {
                    audioSource.clip = musicToPlay;
                }

                audioSource.Play();
                currentZoneIndex = zoneIndex;
                previousClip = musicToPlay; // Store the current clip as the previous valid clip
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
            return 0; // Zone 1
        }
        else if (levelIndex >= 4 && levelIndex <= 6)
        {
            return 1; // Zone 2
        }
        else
        {
            // Handle other cases as needed
            return 0; // Default to Zone 1 for unknown levels
        }
    }

    bool IsMainMenuScene(int levelIndex)
    {
        // Adjust this logic based on the actual build index of your main menu scene
        return levelIndex == 0;
    }

    // Add this method to reset audioSource when a new scene is loaded
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
