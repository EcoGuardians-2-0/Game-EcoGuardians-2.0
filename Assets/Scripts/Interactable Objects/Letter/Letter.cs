using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : InteractableObject
{

    public Sprite targetUIImage;
    private bool toggle;
    private string selectionPromptBefore = "Leer nota";
    private string selectionPromptAfter = "Dejar de leer nota";

    new void Start()
    {
        base.Start();
        selectionPrompt = selectionPromptBefore;

        // Retrieving texture as image of letter
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer != null && renderer.material != null)
        {
            // Converting material to texture
            Texture2D texture = renderer.material.mainTexture as Texture2D;
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Debug.Log("Assigning sprite");
                targetUIImage = sprite;

            }
        }
    }

    public override void Interact()
    {

        DisableObjects.Instance.disableSwitchCamera();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableCharacterController();

        toggle = !toggle;

        if (toggle)
        {
            SelectionManager.instance.canHighlight = false;
            SelectionManager.instance.oneTimeInteraction = true;
            turnOn();
            EventManager.Photograph.OnActiveCamera(false);
            EventManager.QuestUI.OnLockQuestUI();
            EventManager.Minimap.OnLockMiniMap();
        }
        else
        {
            SelectionManager.instance.canHighlight = true;
            SelectionManager.instance.oneTimeInteraction = false;
            turnOff();
            EventManager.Photograph.OnActiveCamera(true);
            EventManager.QuestUI.OnUnlockQuestUI();
            EventManager.Minimap.OnUnlockMiniMap();
        }
    }

    private void turnOn()
    {
        AudioManager.Instance.PlaySound(SoundType.OpenNote);
        EventManager.Letter.OnDisplayLetterImage.Invoke(targetUIImage, true);
        selectionPrompt = selectionPromptAfter;
        CameraUtilityManager.Instance.SetCameraOn(transform, 1.5f, new Vector3(0f, 0.8f, 1.8f), new Vector3(0, 0, 0));
    }

    private void turnOff()
    {
        EventManager.Letter.OnDisplayLetterImage(null, false);
        selectionPrompt = selectionPromptBefore;
        CameraUtilityManager.Instance.SetCameraOff();
    }


}
