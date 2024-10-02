using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : InteractableObject
{
    [SerializeField]
    public GameObject activity;
    [SerializeField]
    private string questID;

    // Singleton pattern
    private static Activity _instance;
    public static Activity Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Activity>();
            }
            return _instance;
        }
    }

    // With the singleton pattern, we can call the Start method from the base class
    new void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        base.Start();

    }


    public override void Interact()
    {
        DisableObjects.Instance.showCursor();
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
        activity.SetActive(true);
        //EventManager.Quest.OnQuestCompleted(questID);
    }
}    