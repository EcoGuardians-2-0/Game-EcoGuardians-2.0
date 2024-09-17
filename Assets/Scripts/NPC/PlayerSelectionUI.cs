using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
    [SerializeField]
    private Transform characterPreviewParent;

    [SerializeField]
    private float turnSpeed = 2.0f;

    private CharacterManager characterManager;

    private List<GameObject> instantiatedPrefabs;

    private int currentCharacterIndex = 0;

    private bool showCharacter;


    void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        instantiatedPrefabs = new List<GameObject>();
        showCharacter = false;
    }

    private void Update()
    {
        if (characterPreviewParent != null && showCharacter)
        {
            characterPreviewParent.RotateAround(
                characterPreviewParent.position,
                characterPreviewParent.up,
                turnSpeed * Time.deltaTime
                );
        }
    }

    public void prepareCharacters()
    {
        instantiatedPrefabs.Clear();

        foreach (GameObject characterInstance in characterManager.getCharacterPrefabs())
        {
            Debug.Log(characterInstance.name);
            GameObject instance = Instantiate(characterInstance, characterPreviewParent.position, characterPreviewParent.rotation);
            instance.transform.SetParent(characterPreviewParent.transform);
            instance.SetActive(false);
            instantiatedPrefabs.Add(instance);
        }

        instantiatedPrefabs[currentCharacterIndex].SetActive(true);
        showCharacter = true;
    }

    public void Right()
    {
        instantiatedPrefabs[currentCharacterIndex].SetActive(false);
        currentCharacterIndex = (currentCharacterIndex + 1) % instantiatedPrefabs.Count;
        instantiatedPrefabs[currentCharacterIndex].SetActive(true);
    }

    public void Left()
    {
        instantiatedPrefabs[currentCharacterIndex].SetActive(false);
        currentCharacterIndex--;

        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = instantiatedPrefabs.Count - 1;  
        }

        instantiatedPrefabs[currentCharacterIndex].SetActive(true);
    }

    public void deletePreviewPrefabs()
    {
        for (int i = 0; i < characterPreviewParent.childCount; i++)
        {
            Destroy(characterPreviewParent.GetChild(i).gameObject);
        }
    }

    public void createCharacter()
    {
        deletePreviewPrefabs();
        showCharacter = false;
        characterManager.spawnCharacter(currentCharacterIndex);
    }
}
