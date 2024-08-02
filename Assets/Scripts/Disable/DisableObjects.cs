using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjects : MonoBehaviour
{

    public static DisableObjects Instance { get; private set;}

    [SerializeField]
    private CinemachineVirtualCamera[] cameras;

    [SerializeField]
    private PlayerController characterController;

    [SerializeField]
    private SwitchCamera switchCamera;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void disableCameras()
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            camera.enabled = !camera.enabled;
        }
    }

    public void disableSwitchCamera()
    {
        switchCamera.enabled = !switchCamera.enabled;
    }

    public void disableCharacterController()
    {
        characterController.isInGame = ! characterController.isInGame;
    }


}
