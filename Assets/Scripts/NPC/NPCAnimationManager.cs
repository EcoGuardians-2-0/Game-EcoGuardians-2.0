using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterAnimatorPair> characterAnimatorList = new List<CharacterAnimatorPair>();

    private Dictionary<string, Animator> characterAnimators = new Dictionary<string, Animator>();

    void Awake()
    {
        foreach (var pair in characterAnimatorList)
        {
            if (!characterAnimators.ContainsKey(pair.characterName))
            {
                characterAnimators.Add(pair.characterName, pair.animator);
            }
        }
    }

    public void setBlendParameter(string characterName,int value,float damping)
    {
        Debug.Log($"Valor pasaddo {value}");
        if (characterAnimators.TryGetValue(characterName, out Animator animator))
        {
            animator.SetFloat("Blend", value, damping, Time.deltaTime);
        }
        else
        {
            Debug.LogWarning($"Character {characterName} not found in NPCAnimationManager");
        }
    }

    public void setBlendParameter(string characterName, int value)
    {
        Debug.Log($"Valor pasaddo {value}");
        if (characterAnimators.TryGetValue(characterName, out Animator animator))
        {
            animator.SetFloat("Blend", value);
        }
        else
        {
            Debug.LogWarning($"Character {characterName} not found in NPCAnimationManager");
        }
    }
}
