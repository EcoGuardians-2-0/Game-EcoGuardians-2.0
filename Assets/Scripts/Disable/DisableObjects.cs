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
    private GameObject selectionToolTip;

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
    private void Start()
    {
        disableCameras();
    }

    public void setPlayer(GameObject player, PlayerController characterController)
    {
        this.player = player;
        this.characterController = characterController;
    }

    public void setSwitchCamera(SwitchCamera switchCamera)
    {
        this.switchCamera = switchCamera;
    }

    public void disableCameras()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
            camera.enabled = !camera.enabled;
    }

    public void EnableCameras()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
            camera.enabled = true;
    }

    // Disable the camera movement
    public void DisableCameraMovement()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            var pov = camera.GetCinemachineComponent<CinemachinePOV>();
            if (pov != null)
            {
                pov.enabled = false;
            }
        }
    }

    public void EnableCameraMovement()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            var pov = camera.GetCinemachineComponent<CinemachinePOV>();
            if (pov != null)
            {
                pov.enabled = true;
            }
        }
    }

    public void disableSwitchCamera()
    {
        switchCamera.enabled = !switchCamera.enabled;
    }

    public void EnableSwitchCamera()
    {
        switchCamera.enabled = true;
    }

    public void disableCharacterController()
    {
        characterController.isInGame = !characterController.isInGame;
    }

    public void EnableCharacerController()
    {
        characterController.isInGame = true;
    }

    public void TogglePlayer()
    {
        player.SetActive(!player.activeSelf);
    }

    // Enable the player
    public void EnablePlayer()
    {
        player.SetActive(true);
    }

    public void ToggleMinimap()
    {
        minimap.SetActive(!minimap.activeSelf);
    }

    public void ToggleSelectionCursor()
    {
        selectionCursor.SetActive(!selectionCursor.activeSelf);
    }

    public void ToggleTooltip()
    {
        selectionToolTip.SetActive(!selectionToolTip.activeSelf);
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

    public void showCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void hideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ForceDisableControlsImageTVUI() { AnimationsControlsTV.Instance.HideControlsImageTVUI(true); }

    public void ForceDisableControlsVideoTVUI() { AnimationsControlsTV.Instance.HideControlsVideoTVUI(true); }

    //Disable everything
    public void DisableAll()
    {
        disableCameras();
        disableSwitchCamera();
        disableCharacterController();
        TogglePlayer();
        ToggleMinimap();
        ToggleSelectionCursor();
        DisableCameraMovement();
        showCursor();
    }

    public void EnableWorld()
    {
        hideCursor();
        EnableCharacerController();
        EnableCameras();
        EnableSwitchCamera();
    }
}
