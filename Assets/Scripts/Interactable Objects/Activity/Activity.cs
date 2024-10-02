using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : InteractableObject
{
    [SerializeField]
    private GameObject activity;
    [SerializeField]
    private string questID;

    new void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
/*        DisableObjects.Instance.showCursor();
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
        activity.SetActive(true);*/
        EventManager.Quest.OnQuestCompleted(questID);
    }
}    