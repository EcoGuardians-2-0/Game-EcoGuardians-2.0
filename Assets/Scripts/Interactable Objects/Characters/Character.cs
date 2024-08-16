using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    [Header("INK JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    [SerializeField]
    private DialogueData npc;

    public override void Interact()
    {

        if (!DialogueManager.instance.isTalking)
        {
            SelectionManager.instance.isInteracting = true;
            DisableObjects.Instance.disableCharacterController();
            DisableObjects.Instance.disableCameras();
            DisableObjects.Instance.disableSwitchCamera();
            DialogueManager.instance.StartConversation(inkJSON);
        }

    }

}
