using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    [SerializeField]
    private DialogueData npc;

    public override void Interact()
    {
        base.Interact();

        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();

        DialogueManager.instance.StartConversation(npc, this);
    }

}
