using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.group;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.ignoreListenerPause = s.uiSound;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) Play("MainTheme");
        if (SceneManager.GetActiveScene().buildIndex == 2) Play("TheDreamlands");
        if (SceneManager.GetActiveScene().buildIndex == 3) Play("TheCave");
        if (SceneManager.GetActiveScene().buildIndex == 4) Play("TheForest");
        if (SceneManager.GetActiveScene().buildIndex == 5) Play("TheNightmare");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }

    public void FadeIn(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        StartCoroutine(In(s.source, 3f));

        IEnumerator In(AudioSource audioSource, float FadeTime)
        {
            audioSource.Play();
            audioSource.volume = 0f;
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }

    public void FadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        StartCoroutine(Out(s.source, 3f));

        IEnumerator Out(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            audioSource.Stop();
        }
    }
}
