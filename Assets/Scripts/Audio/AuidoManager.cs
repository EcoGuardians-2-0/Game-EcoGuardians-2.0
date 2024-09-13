using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource globalAudioSource, musicAudioSource;
    [SerializeField] private SoundsSO soundCollection;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private Dictionary<string, FootstepCollection> footstepCollections;
    private string currentSurfaceType = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlaySound(SoundType.environment, musicAudioSource);
    }

    private void InitializeAudioManager()
    {
        if (globalAudioSource == null)
        {
            globalAudioSource = gameObject.AddComponent<AudioSource>();
        }
        LoadFootstepCollections();
    }

    private void LoadFootstepCollections()
    {
        footstepCollections = new Dictionary<string, FootstepCollection>();
        FootstepCollection[] collections = Resources.LoadAll<FootstepCollection>("FootstepsCollection");
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
        AudioSource targetSource = source ? source : globalAudioSource;

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
            globalAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayJumpSound()
    {
        if (footstepCollections.TryGetValue(currentSurfaceType, out FootstepCollection collection) && collection.jumpSound != null)
        {
            globalAudioSource.PlayOneShot(collection.jumpSound);
        }
    }

    public void PlayLandSound()
    {
        if (footstepCollections.TryGetValue(currentSurfaceType, out FootstepCollection collection) && collection.landSound != null)
        {
            globalAudioSource.PlayOneShot(collection.landSound);
        }
    }
}