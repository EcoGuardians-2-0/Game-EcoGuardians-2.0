using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : InteractableObject
{

    public GameObject letterUI;
    bool toggle;

    public override void Interact()
    {
        toggle = !toggle;
        if (toggle)
        {
            DisableObjects.Instance.disableCharacterController();
            DisableObjects.Instance.disableSwitchCamera();
            DisableObjects.Instance.disableCameras();
            letterUI.SetActive(true);
        }
        else
        {
            DisableObjects.Instance.disableCharacterController();
            DisableObjects.Instance.disableSwitchCamera();
            DisableObjects.Instance.disableCameras();
            letterUI.SetActive(false);
        }
    }
}
