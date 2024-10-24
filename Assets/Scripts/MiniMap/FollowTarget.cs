using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject player; // The player
    public float yOffset = 0f; // Y offset to place the object above the player

    private PlayerController playerController; // Reference to the PlayerController script
    private float fixedY; // The fixed Y position for the object

    private SwitchCamera switchCamera;
    private CinemachineVirtualCamera firstCam;
    private CinemachineVirtualCamera thirdCam;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;

        // Get the parent object (Character model)
        player = this.gameObject.transform.parent.gameObject;
        // Get the reference to the PlayerController of the player
        playerController = player.GetComponent<PlayerController>();

        // Initialize the fixed Y position
        fixedY = player.transform.position.y + yOffset;

        // Place the object at the initial correct position
        Vector3 initialPosition = player.transform.position;
        initialPosition.y = fixedY;
        transform.position = initialPosition;

        switchCamera = player.GetComponent<SwitchCamera>();
        if (switchCamera != null)
        {
            firstCam = switchCamera.FirstCam;
            thirdCam = switchCamera.ThirdCam;
        }

        // Adjust rotation only if cameras are initialized
        if (firstCam != null && thirdCam != null)
            AdjustRotation();
    }

    void Update()
    {
        if ( firstCam == null)
            firstCam = switchCamera.FirstCam;

        if (thirdCam == null)
            thirdCam = switchCamera.ThirdCam;

        // Get the player's position
        Vector3 playerPosition = player.transform.position;

        // Check if the player is grounded
        if (playerController.GetIsGrounded())
            fixedY = playerPosition.y + yOffset; // Update the fixed Y position when the player is grounded

        // Keep the object above the player in X and Z, but with a fixed Y if not grounded
        Vector3 newPosition = new Vector3(playerPosition.x, fixedY, playerPosition.z);
        transform.position = newPosition;

        // Adjust rotation only if cameras are initialized
        if (firstCam != null && thirdCam != null)
            AdjustRotation();
    }

    void AdjustRotation()
    {
        if (firstCam.Priority == 11 || thirdCam.Priority == 11)
        {
            float cameraRotationY = mainCamera.rotation.eulerAngles.y;
            Vector3 currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, cameraRotationY, currentRotation.z);
        }
    }
}
