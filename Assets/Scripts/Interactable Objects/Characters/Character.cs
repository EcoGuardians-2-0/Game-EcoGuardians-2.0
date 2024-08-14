using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
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
            DialogueManager.instance.StartConversation(npc, this);
        }
        else
        {
            if (DialogueManager.instance.isDone)
            {
                SelectionManager.instance.isInteracting = false;
                DisableObjects.Instance.disableCharacterController();
                DisableObjects.Instance.disableCameras();
                DisableObjects.Instance.disableSwitchCamera();
                DialogueManager.instance.EndDialogue();
            }

        }

    }

}
