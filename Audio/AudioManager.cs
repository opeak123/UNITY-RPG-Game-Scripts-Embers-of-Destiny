using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer mixer;

    public AudioSource[] sfxSources;
    public AudioSource[] bgmSources;

    [HideInInspector]
    public AudioClip[] bgmClips;
    [HideInInspector]
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySFX(int index, float volume)
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                source.clip = sfxClips[index];
                source.volume = volume;
                source.Play();
                return;
            }
        }
    }

    public void PlayBGM(int index, float volume)
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Stop();
        }

        foreach (AudioSource source in bgmSources)
        {
            if (source.clip == null || source.clip != bgmClips[index])
            {
                source.clip = bgmClips[index];
                source.volume = volume;
                source.loop = true;
                source.Play();
                return;
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGMVolume", volume);
    }
}
