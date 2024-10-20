using System;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(fileName = "New Sound Collection", menuName = "Create New Sound Collection")]

public class SoundsSO : ScriptableObject
{
    // General audio mixers
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public SoundList[] sounds;
}

[Serializable]
public struct SoundList
{
    [SerializeField] public string name;
    [Range(0, 1)] public float volume;
    public AudioMixerGroup mixer;
    public AudioClip[] sounds;
}