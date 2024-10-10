using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : InteractableObject
{
    private GameObject onObject;
    private GameObject offObject;

    private DisableObjects disableObjects;
    private CameraUtilityManager cameraUtilityManager;

    private TVImagesManager tvImagesManager;
    private TVVideoManager tvVideoManager;

    private string selectionPromptBefore = "Ver TV";
    private string selectionPromptAfter = "Dejar de ver";

    private bool isOn;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start(); // Do not remove - child calls parent method
        isOn = false;

        if (!LoadObjects())
            Debug.LogError(gameObject.name + ": No se encontraron todos los objetos necesarios.");
        else
        {
            onObject.SetActive(false);
            offObject.SetActive(true);

            selectionPrompt = selectionPromptBefore;

            disableObjects = DisableObjects.Instance;
            cameraUtilityManager = CameraUtilityManager.Instance;

            tvImagesManager = onObject.GetComponent<TVImagesManager>();
            tvVideoManager = onObject.GetComponent<TVVideoManager>();
        }
    }

    // Interact with the TV
    public override void Interact()
    {
        base.Interact(); // Do not remove - child calls parent method

        disableObjects.TogglePlayer();
        disableObjects.ToggleSelectionCursor();

        onObject.SetActive(!onObject.activeSelf);
        offObject.SetActive(!offObject.activeSelf);

        disableObjects.disableSwitchCamera();
        disableObjects.disableCameras();
        disableObjects.disableCharacterController();

        isOn = !isOn;

        if (isOn){
            EventManager.Minimap.OnLockMiniMap.Invoke();
            EventManager.Photograph.OnActiveCamera(false);
            AudioManager.Instance.PlaySound(SoundType.TVOn);
            ActiveTV();
        }
        else
        {
            EventManager.Minimap.OnUnlockMiniMap.Invoke();
            EventManager.Photograph.OnActiveCamera(true);
            AudioManager.Instance.PlaySound(SoundType.TvOff);
            DeactivateTV();
        }
    }

    // Turn on the TV
    private void ActiveTV()
    {
        cameraUtilityManager.SetCameraOn(onObject.transform, 2f, new Vector3(0, 0, 1.05f), new Vector3(0, 0, 0));

        selectionPrompt = selectionPromptAfter;

        if (tvImagesManager != null)
            tvImagesManager.Init();

        if (tvVideoManager != null)
        {
            if (!tvVideoManager.hasVideo)
                tvVideoManager.PlayVideo();

            if (tvVideoManager.hasVideo)
                tvVideoManager.PlayVideo();
        }
    }

    // Turn off the TV
    private void DeactivateTV()
    {
        cameraUtilityManager.SetCameraOff();

        selectionPrompt = selectionPromptBefore;

        if (tvVideoManager != null)
        {
            if (tvVideoManager.hasVideo)
            {
                disableObjects.ForceDisableControlsVideoTVUI();
                tvVideoManager.PauseVideo();
            }
        }
        else if (tvImagesManager != null)
        {
            disableObjects.ForceDisableControlsImageTVUI();
            tvImagesManager.TurnOff();
        }
    }

    // Load the objects needed for the TV
    private bool LoadObjects()
    {
        onObject = transform.Find("On")?.gameObject;
        offObject = transform.Find("Off")?.gameObject;

        if (onObject == null || offObject == null)
            return false;

        return true;
    }
}