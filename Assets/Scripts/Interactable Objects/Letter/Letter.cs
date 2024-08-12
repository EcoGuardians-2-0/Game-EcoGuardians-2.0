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
        base.Interact(); // Do not remove - child calls parent method

        DisableObjects.Instance.disableSwitchCamera();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableCharacterController();

        toggle = !toggle;
        letterUI.SetActive(toggle);
    }
}
