using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Audio sources
    private AudioSource sfxAudioSource;
    private AudioSource musicAudioSource;

    // Sound collection
    private SoundsSO soundCollection;

    private Dictionary<string, FootstepCollection> footstepCollections;
    private string currentSurfaceType = "";

    // This is really the only blurb of code you need to implement a Unity singleton
    private static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<AudioManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeAudioManager();
        PlayMusic(SoundType.MainMenuBackgroundMusic);
    }

    private void InitializeAudioManager()
    {
        Debug.Log("Hello there");
        // Load the SoundsSO scriptable object from the Resources folder
        soundCollection = Resources.Load<SoundsSO>("Audio/SoundCollection");

        if (soundCollection == null)
        {
            Debug.LogError("SoundCollection not found in Resources folder!");
        }
        else
        {
            // Initialize AudioSource components
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource.outputAudioMixerGroup = soundCollection.sfxMixer;

            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.outputAudioMixerGroup = soundCollection.musicMixer;
        }

        LoadFootstepCollections();
    }

    private void LoadFootstepCollections()
    {
        footstepCollections = new Dictionary<string, FootstepCollection>();
        FootstepCollection[] collections = Resources.LoadAll<FootstepCollection>("FootStepsCollection");
        foreach (var collection in collections)
        {
            footstepCollections[collection.name] = collection;
        }
    }

    public void PlaySound(SoundType soundType, AudioSource source = null, float volume = 1f)
    {
        if (soundCollection == null || soundCollection.sounds == null)
        {
            Debug.LogError("SoundCollection not set or is empty!");
            return;
        }

        SoundList soundList = soundCollection.sounds[(int)soundType];
        if (soundList.sounds == null || soundList.sounds.Length == 0)
        {
            Debug.LogWarning($"No sounds found for SoundType: {soundType}");
            return;
        }

        AudioClip randomClip = soundList.sounds[UnityEngine.Random.Range(0, soundList.sounds.Length)];
        AudioSource targetSource = source ? source : sfxAudioSource;

        targetSource.outputAudioMixerGroup = soundList.mixer;
        targetSource.PlayOneShot(randomClip, volume * soundList.volume);
    }

    public void UpdateSurfaceType(string surfaceType)
    {
        currentSurfaceType = string.IsNullOrEmpty(surfaceType) ? "" : surfaceType;
    }

    public void PlayFootstepSound(bool isRunning)
    {
        if (!footstepCollections.TryGetValue(currentSurfaceType, out FootstepCollection collection))
        {
            Debug.LogWarning($"No FootstepCollection found for surface type: {currentSurfaceType}");
            return;
        }

        List<AudioClip> sounds = isRunning ? collection.runSounds : collection.footstepsSounds;
        if (sounds != null && sounds.Count > 0)
        {
            AudioClip clip = sounds[UnityEngine.Random.Range(0, sounds.Count)];
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayJumpSound()
    {
        if (footstepCollections.TryGetValue(currentSurfaceType, out FootstepCollection collection) && collection.jumpSound != null)
        {
            sfxAudioSource.PlayOneShot(collection.jumpSound);
        }
    }

    public void PlayLandSound()
    {
        if (footstepCollections.TryGetValue(currentSurfaceType, out FootstepCollection collection) && collection.landSound != null)
        {
            sfxAudioSource.PlayOneShot(collection.landSound);
        }
    }

    public void PlayMusic(SoundType soundType)
    {
        if (soundCollection == null || soundCollection.sounds == null)
        {
            Debug.LogError("SoundCollection not set or is empty!");
            return;
        }
    
        SoundList soundList = soundCollection.sounds[(int)soundType];
        if (soundList.sounds == null || soundList.sounds.Length == 0)
        {
            Debug.LogWarning($"No sounds found for SoundType: {soundType}");
            return;
        }
    
        AudioClip randomClip = soundList.sounds[UnityEngine.Random.Range(0, soundList.sounds.Length)];
        musicAudioSource.clip = randomClip;
        musicAudioSource.outputAudioMixerGroup = soundList.mixer;
        musicAudioSource.volume = soundList.volume;
        // Set loop to true so the music plays indefinitely
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayUIHoverSound()
    {
        PlaySound(SoundType.uiHover);
    }

    public void PlayUISelectSound()
    {
        PlaySound(SoundType.UIbtnSelected);
    }
}