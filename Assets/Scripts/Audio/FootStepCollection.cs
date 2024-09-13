using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Footstep Collection", menuName = "Create New Footstep Collection")]

public class FootstepCollection : ScriptableObject
{
    public List<AudioClip> footstepsSounds = new List<AudioClip>();
    public List<AudioClip> runSounds = new List<AudioClip>();
    public AudioClip jumpSound;
    public AudioClip landSound;
}