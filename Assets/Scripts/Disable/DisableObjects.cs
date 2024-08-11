using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjects : MonoBehaviour
{

    public static DisableObjects Instance { get; private set; }

    [SerializeField]
    private CinemachineVirtualCamera[] cameras;

    [SerializeField]
    private PlayerController characterController;

    [SerializeField]
    private SwitchCamera switchCamera;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject minimap;

    [SerializeField]
    private GameObject selectionCursor;

    [SerializeField]
    private GameObject controlsImageTVUI;

    [SerializeField]
    private GameObject controlsVideoTVUI;

    private bool isFirstTimeImageTV = true;
    private bool isFirstTimeVideoTV = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void disableCameras()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
            camera.enabled = !camera.enabled;
    }

    public void disableSwitchCamera()
    {
        switchCamera.enabled = !switchCamera.enabled;
    }

    public void disableCharacterController()
    {
        characterController.isInGame = !characterController.isInGame;
    }

    public void TogglePlayer()
    {
        player.SetActive(!player.activeSelf);
    }

    public void ToggleMinimap()
    {
        //minimap.SetActive(!minimap.activeSelf);
    }

    public void ToggleSelectionCursor()
    {
        selectionCursor.SetActive(!selectionCursor.activeSelf);
    }

    public void ToggleControlsImageTVUI(bool firstTime)
    {
        if (firstTime)
        {
            if (isFirstTimeImageTV)
                AnimationsControlsTV.Instance.ShowControlsImageTVUI();
        }
        else
        {
            if (AnimationsControlsTV.Instance.GetImageState())
                AnimationsControlsTV.Instance.HideControlsImageTVUI(false);
            else
                AnimationsControlsTV.Instance.ShowControlsImageTVUI();

            isFirstTimeImageTV = !isFirstTimeImageTV;
        }
    }

    public void ToggleControlsVideoTVUI(bool firstTime)
    {
        if (firstTime)
        {
            if (isFirstTimeVideoTV)
                AnimationsControlsTV.Instance.ShowControlsVideoTVUI();
        }
        else
        {
            if (AnimationsControlsTV.Instance.GetVideoState())
                AnimationsControlsTV.Instance.HideControlsVideoTVUI(false);
            else
                AnimationsControlsTV.Instance.ShowControlsVideoTVUI();

            isFirstTimeVideoTV = !isFirstTimeVideoTV;
        }
    }

    public void ForceDisableControlsImageTVUI() { AnimationsControlsTV.Instance.HideControlsImageTVUI(true); }

    public void ForceDisableControlsVideoTVUI() { AnimationsControlsTV.Instance.HideControlsVideoTVUI(true); }
}
