using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : InteractableObject
{
    [SerializeField]
    public GameObject activityOne;
    [SerializeField]
    public GameObject activityTwo;
    [SerializeField]
    public GameObject activityThree;
    [SerializeField]
    private string ActOnequestID;
    [SerializeField]
    private string ActTwoquestID;
    [SerializeField]
    private string ActThreequestID;

    // Remove Singleton pattern
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

    new void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        DisableObjects.Instance.showCursor();
        DisableObjects.Instance.disableCharacterController();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableSwitchCamera();
        SelectionManager.instance.canHighlight = false;
        SelectionManager.instance.isInteracting = true;
        
        if(itemName == "Activity1"){
            activityOne.SetActive(true);
            AudioManager.Instance.PlayMusic(SoundType.Act1BackgroundMusic);
        }
        else if(itemName == "Activity2"){
            activityTwo.SetActive(true);
            AudioManager.Instance.PlayMusic(SoundType.Act2BackgroundMusic);
        }
        else if(itemName == "Activity3"){
            activityThree.SetActive(true);
            AudioManager.Instance.PlayMusic(SoundType.Act3BackgroundMusic);
        }
        else
        {
            Debug.LogError("Activity not found");
        }       
    }
}