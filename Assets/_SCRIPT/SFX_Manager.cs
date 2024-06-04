using UnityEngine;
using System.Collections.Generic;

public class SFX_Manager : MonoBehaviour
{
    public static SFX_Manager Instance;

    public List<AudioClip> soundClips;
    public float volume = 1.0f;
    private bool isMuted;

    private Dictionary<string, AudioClip> audioClipsByName;
    private List<AudioSource> audioSources;

    private void Awake()
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

        audioSources = new List<AudioSource>();
        audioClipsByName = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in soundClips)
        {
            audioClipsByName.Add(clip.name, clip);
        }

        foreach (AudioClip clip in soundClips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = clip;
            source.volume = volume;
            audioSources.Add(source);
        }
    }

    public void PlaySound(string clipName)
    {
        if (isMuted || !audioClipsByName.ContainsKey(clipName))
            return;

        AudioClip clip = audioClipsByName[clipName];
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
    }

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

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        foreach (AudioSource source in audioSources)
        {
            source.mute = isMuted;
        }
    }

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
