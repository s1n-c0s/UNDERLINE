using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [Header("Audio Settings")]
    public AudioSource[] audioSources; // Array to hold multiple AudioSources
    public AudioClip[] soundEffects;  // Array to hold different sound effects
    public float minPitch = 0.9f;     // Minimum pitch value
    public float maxPitch = 1.1f;     // Maximum pitch value
    public int numAudioSources = 5;   // Number of AudioSources to create if needed

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize AudioSources if they are not already assigned
    private void InitializeAudioSources()
    {
        if (audioSources.Length == 0)
        {
            audioSources = new AudioSource[numAudioSources];

            for (int i = 0; i < numAudioSources; i++)
            {
                GameObject audioSourceObject = new GameObject($"AudioSource{i}");
                audioSourceObject.transform.SetParent(transform);
                audioSources[i] = audioSourceObject.AddComponent<AudioSource>();
            }
        }
    }

    // Play a sound effect by index with random pitch
    public void PlaySoundEffect(int index)
    {
        if (AudioManager.instance != null && !AudioManager.instance.IsMuted)
        {
            if (index >= 0 && index < soundEffects.Length)
            {
                AudioSource source = GetAvailableAudioSource();
                if (source != null)
                {
                    source.clip = soundEffects[index];
                    source.pitch = Random.Range(minPitch, maxPitch);
                    source.Play();
                }
                else
                {
                    Debug.LogWarning("No available AudioSource to play the sound effect.");
                }
            }
            else
            {
                Debug.LogError($"Sound effect index {index} out of range.");
            }
        }
    }

    // Stop all currently playing sound effects
    public void ClearAllSoundEffects()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    // Get an available AudioSource
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // If no AudioSource is available, return null
        return null;
    }
}
