﻿using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum BackgroundMusic
    {
        MainMenu,
        Ambient,
    }

    public enum SoundEffect
    {
        ClickToStart,
        Reload,
        Fire,
        Enemy_Fire,
        TrafficLight,
        HitEnemy,
        Enemy_FireM113,
        DroneImpact,
        DroneMoving,
        ButtonClick,
    }

    // For initializing sounds
    public AudioClip[] BackgroundMusicToLoad = new AudioClip[Enum.GetNames(typeof(BackgroundMusic)).Length];

    public AudioClip[] SoundEffectsToLoad = new AudioClip[Enum.GetNames(typeof(SoundEffect)).Length];

    // For Controlling Volume
    private static float sfxVolume = 1.0f;

    private static float bgmVolume = 1.0f;

    // For Controlling Speed
    private const float MIN_SOUND_SPEED = 0.2f;       // So that the sound speed doesn't go too low to the point where it can't be heard

    public static float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            // Ensure the value for volume is within range
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            sfxVolume = value;
            SetEffectVolume(sfxVolume);
        }
    }

    public static float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            // Ensure the value for volume is within range
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            bgmVolume = value;
            SetBGMVolume(bgmVolume);
        }
    }

    // For Singleton
    private static bool instantiated = false;

    // For managing audio state
    private static AudioSource[] bgmList = new AudioSource[Enum.GetNames(typeof(BackgroundMusic)).Length];

    private static AudioSource[] sfxList = new AudioSource[Enum.GetNames(typeof(SoundEffect)).Length];
    private static AudioSource currentBGM;

    // Use this for initialization
    private void Start()
    {
        // Don't allow loading of SoundManager again
        if (instantiated)
        {
            return;
        }

        // SoundManager should be persistent
        DontDestroyOnLoad(this);
        instantiated = true;

        // Initialize all the BGMs
        for (int i = 0; i < Enum.GetNames(typeof(BackgroundMusic)).Length; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            bgmList[i] = go.AddComponent<AudioSource>();
            bgmList[i].clip = BackgroundMusicToLoad[i];
            bgmList[i].loop = true;
        }

        // Initialize all the SFXs
        for (int i = 0; i < Enum.GetNames(typeof(SoundEffect)).Length; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            sfxList[i] = go.AddComponent<AudioSource>();
            sfxList[i].clip = SoundEffectsToLoad[i];
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    static public void PlaySoundEffect(SoundEffect sfx)
    {
        if (sfx == SoundEffect.Fire)
        {
            sfxList[(int)sfx].PlayOneShot(sfxList[(int)sfx].clip);
        }
        else
        {
            sfxList[(int)sfx].Play();
        }
    }

    static public void PlayBackgroundMusic(BackgroundMusic bgm)
    {
        // If a BGM is playing, stop the previous
        if (currentBGM)
        {
            currentBGM.Stop();
            currentBGM = null;
        }

        // Set the new BGM as the current
        currentBGM = bgmList[(int)bgm];
        currentBGM.Play();
    }

    static public void SetEffectVolume(float vol)
    {
        // Clamp the value so that it is within the limits
        sfxVolume = Mathf.Clamp(vol, 0.0f, 1.0f);

        foreach (AudioSource snd in sfxList)
        {
            snd.volume = sfxVolume;
        }
    }

    static public void SetBGMVolume(float vol)
    {
        // Clamp the value so that it is within the limits
        bgmVolume = Mathf.Clamp(vol, 0.0f, 1.0f);

        foreach (AudioSource snd in bgmList)
        {
            snd.volume = bgmVolume;
        }
    }

    static public void MuteAll()
    {
        SetEffectVolume(0.0f);
        SetBGMVolume(0.0f);
    }

    static public void UnmuteAll()
    {
        SetEffectVolume(sfxVolume);
        SetBGMVolume(bgmVolume);
    }

    static public void ChangeBGMSpeed(float speed)
    {
        // Clamp the value so that it is within the limits
        speed = Mathf.Clamp(speed, MIN_SOUND_SPEED, 1.0f);

        foreach (AudioSource snd in bgmList)
        {
            snd.pitch = speed;
        }
    }

    static public void ChangeSFXSpeed(float speed)
    {
        // Clamp the value so that it is within the limits
        speed = Mathf.Clamp(speed, MIN_SOUND_SPEED, 1.0f);

        foreach (AudioSource snd in sfxList)
        {
            snd.pitch = speed;
        }
    }
}