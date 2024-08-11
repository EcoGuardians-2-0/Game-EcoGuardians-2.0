using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject player; // The player
    public float yOffset = 1.0f; // Y offset to place the object above the player

    private PlayerController playerController; // Reference to the PlayerController script
    private float fixedY; // The fixed Y position for the object

    void Start()
    {
        // Get the reference to the PlayerController of the player
        playerController = player.GetComponent<PlayerController>();

        // Initialize the fixed Y position
        fixedY = player.transform.position.y + yOffset;

        // Place the object at the initial correct position
        Vector3 initialPosition = player.transform.position;
        initialPosition.y = fixedY;
        transform.position = initialPosition;

        // Adjust the initial rotation of the object to match the player's Y rotation
        AdjustRotation();
    }

    void Update()
    {
        // Get the player's position
        Vector3 playerPosition = player.transform.position;

        // Check if the player is grounded
        bool isGrounded = playerController.GetIsGrounded();

        if (isGrounded)
        {
            // Update the fixed Y position when the player is grounded
            fixedY = playerPosition.y + yOffset;
        }

        // Keep the object above the player in X and Z, but with a fixed Y if not grounded
        Vector3 newPosition = new Vector3(playerPosition.x, fixedY, playerPosition.z);
        transform.position = newPosition;

        // Adjust the object's rotation to match the player's Y rotation
        AdjustRotation();
    }

    void AdjustRotation()
    {
        // Get the player's Y rotation
        float playerRotationY = player.transform.rotation.eulerAngles.y;

        // Maintain the object's original X and Z rotations
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, playerRotationY, currentRotation.z);
    }
}
