using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource audioSource;
    public AudioClip[] soundEffects; // Array to hold different sound effects
    public float minPitch = 0.9f; // Minimum pitch value
    public float maxPitch = 1.1f; // Maximum pitch value

    private void Awake()
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

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Method to play a sound effect by index with random pitch
    public void PlaySoundEffect(int index)
    {
        if (AudioManager.instance != null && !AudioManager.instance.IsMuted)
        {
            if (index >= 0 && index < soundEffects.Length)
            {
                audioSource.clip = soundEffects[index];
                audioSource.pitch = Random.Range(minPitch, maxPitch); // Set random pitch
                audioSource.Play(); // Play the sound effect
            }
            else
            {
                Debug.LogError("Sound effect index out of range: " + index);
            }
        }
    }
}