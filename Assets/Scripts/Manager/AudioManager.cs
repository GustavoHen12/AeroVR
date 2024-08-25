using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip sfxWind;
    public AudioClip sfxCrash;

    public void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();

        PlayWind();
    }

    public void PlayCrash()
    {
        sfxSource.Stop();
        sfxSource.loop = false;
        sfxSource.clip = sfxCrash;
        sfxSource.Play();
    }

    public void PlayWind()
    {
        sfxSource.Stop();
        sfxSource.clip = sfxWind;
        sfxSource.Play();
    }
}
