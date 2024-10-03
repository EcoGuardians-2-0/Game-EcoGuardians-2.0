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
        base.Interact(); // Do not remove - child calls parent method

        DisableObjects.Instance.disableSwitchCamera();
        DisableObjects.Instance.disableCameras();
        DisableObjects.Instance.disableCharacterController();

        toggle = !toggle;

        if (toggle)
            turnOn();
        else
            turnOff();
    }

    private void turnOn()
    {
        AudioManager.Instance.PlaySound(SoundType.OpenNote);
        EventManager.Letter.OnDisplayLetterImage.Invoke(targetUIImage, true);
        selectionPrompt = selectionPromptAfter;
    }

    private void turnOff()
    {
        EventManager.Letter.OnDisplayLetterImage(null, false);
        selectionPrompt = selectionPromptBefore;
    }


}
