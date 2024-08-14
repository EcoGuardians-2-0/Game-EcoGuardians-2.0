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



        if (!DialogueManager.instance.isTalking)
        {
            DisableObjects.Instance.disableCharacterController();
            DisableObjects.Instance.disableCameras();
            DisableObjects.Instance.disableSwitchCamera();
            DialogueManager.instance.StartConversation(npc, this);
        }
        else
        {
            if (DialogueManager.instance.isDone)
            {
                DisableObjects.Instance.disableCharacterController();
                DisableObjects.Instance.disableCameras();
                DisableObjects.Instance.disableSwitchCamera();
                DialogueManager.instance.EndDialogue();
            }

        }

    }

}
