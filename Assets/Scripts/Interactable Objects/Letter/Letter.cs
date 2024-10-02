using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : InteractableObject
{

    public GameObject letterUI;
    private bool toggle;
    private string selectionPromptBefore = "Leer nota";
    private string selectionPromptAfter = "Dejar de leer nota";

    new void Start()
    {
        base.Start();
        selectionPrompt = selectionPromptBefore;
    }

    public override void Interact()
    {
        base.Interact(); // Do not remove - child calls parent method

        DisableObjects.Instance.disableSwitchCamera();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableCharacterController();

        toggle = !toggle;

        letterUI.SetActive(toggle);

        if (toggle)
            turnOn();
        else
            turnOff();
    }

    private void turnOn()
    {
        AudioManager.Instance.PlaySound(SoundType.OpenNote);
        selectionPrompt = selectionPromptAfter;
    }

    private void turnOff()
    {
        selectionPrompt = selectionPromptBefore;
    }


}
