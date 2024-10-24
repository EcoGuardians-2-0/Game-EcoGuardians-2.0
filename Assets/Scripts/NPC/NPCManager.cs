using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager: MonoBehaviour {

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private List<GameObject> characterPrefabs;

    [SerializeField]
    private Transform mainCamera;

    [SerializeField]
    private List<CinemachineVirtualCamera> characterCameras;

    private GameObject currentCharacter;

    private string characterTag = "Player";

    public void spawnCharacter(int index)
    {
        if (characterPrefabs[index] != null && spawnPoint != null)
        {
            currentCharacter = Instantiate(characterPrefabs[index], spawnPoint.position, spawnPoint.rotation);
            currentCharacter.transform.SetParent(spawnPoint);
            currentCharacter.tag = characterTag;
            currentCharacter.name = characterPrefabs[index].name;

            if (currentCharacter.GetComponent<CharacterController>() == null)
            {
                currentCharacter.AddComponent<CharacterController>();
                currentCharacter.GetComponent<CharacterController>().center = new Vector3(0, 1, 0);
                currentCharacter.GetComponent<CharacterController>().radius = 0.2f;
                currentCharacter.GetComponent<CharacterController>().height = 1.85f;
            }

            if (currentCharacter.GetComponent<PlayerController>() == null)
            {
                currentCharacter.AddComponent<PlayerController>();
            }

            if (currentCharacter.GetComponent<StepCall>() == null)
            {
                currentCharacter.AddComponent<StepCall>();
            }

            if (currentCharacter.GetComponent<PositionController>() == null)
            {
                currentCharacter.AddComponent<PositionController>();
            }

            // Creating game object to create the character icon on the minimap screen
            GameObject miniMapComponent = new GameObject("MinimapIcon");
            miniMapComponent.transform.SetParent(currentCharacter.transform);
            if (miniMapComponent.GetComponent<MiniMapComponent>() == null)
            {
                MiniMapComponent miniMap = miniMapComponent.AddComponent<MiniMapComponent>();
                miniMap.enabled = false;
                miniMap.icon = Resources.Load<Sprite>("Sprites/Minimap/playerIcon");
                miniMap.size = new Vector2(30, 30);
                miniMap.enabled = true;
            }

            // Adding the folow target to the minimap game object
            if (miniMapComponent.GetComponent<FollowTarget>() == null)
            {
                FollowTarget followTarget = miniMapComponent.AddComponent<FollowTarget>();
            }

            MiniMapController.changeTarget(miniMapComponent.transform);

            currentCharacter.GetComponent<PlayerController>().setCameraTransform(mainCamera);

            // Third Person Camera
            characterCameras[0].Follow = currentCharacter.transform.GetChild(2);
            characterCameras[0].LookAt = currentCharacter.transform.GetChild(2);

            // First Person Camera
            characterCameras[1].Follow = currentCharacter.transform.GetChild(1);

            // Setting disable object's player
            DisableObjects.Instance.setPlayer(spawnPoint.gameObject, currentCharacter.GetComponent<PlayerController>());

            // Switch camera script
            if (currentCharacter.GetComponent<SwitchCamera>() == null)
            {
                currentCharacter.AddComponent<SwitchCamera>();
                currentCharacter.GetComponent<SwitchCamera>().ThirdCam = characterCameras[0];
                currentCharacter.GetComponent<SwitchCamera>().FirstCam = characterCameras[1];
            }
            DisableObjects.Instance.setSwitchCamera(currentCharacter.GetComponent<SwitchCamera>());

        }
    }

    public void destroyCharacter()
    {
        Destroy(currentCharacter);
    }

    public List<GameObject> getCharacterPrefabs()
    {
        return this.characterPrefabs;
    }

    

}
