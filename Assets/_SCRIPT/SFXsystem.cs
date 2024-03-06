using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> soundClips; // List of audio clips to play
    public float volume = 1.0f; // Volume level of the sound effects
    public bool isSoundOn = true; // Flag to control sound on/off

    private Dictionary<string, AudioClip> audioClipsByName;
    private List<AudioSource> audioSources;

    void Start()
    {
        // Initialize the list of AudioSources
        audioSources = new List<AudioSource>();

        // Initialize the dictionary of audio clips by name
        audioClipsByName = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundClips)
        {
            audioClipsByName.Add(clip.name, clip);
        }

        // Create an AudioSource for each sound clip
        foreach (AudioClip clip in soundClips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = clip;
            source.volume = volume;
            audioSources.Add(source);
        }
    }

    // Function to play a specific sound effect by name
    public void PlaySound(string clipName)
    {
        if (!isSoundOn || !audioClipsByName.ContainsKey(clipName))
            return;

        AudioClip clip = audioClipsByName[clipName];
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

    // Function to pause a specific sound effect by name
    public void PauseSound(string clipName)
    {
        if (audioClipsByName.ContainsKey(clipName))
        {
            foreach (AudioSource source in audioSources)
            {
                if (source.clip.name == clipName && source.isPlaying)
                {
                    source.Pause();
                    break;
                }
            }
        }
    }

    // Function to stop a specific sound effect by name
    public void StopSound(string clipName)
    {
        if (audioClipsByName.ContainsKey(clipName))
        {
            foreach (AudioSource source in audioSources)
            {
                if (source.clip.name == clipName && source.isPlaying)
                {
                    source.Stop();
                    break;
                }
            }
        }
    }

    // Function to toggle sound on/off
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
    }

    // Helper function to get an available AudioSource
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
}
