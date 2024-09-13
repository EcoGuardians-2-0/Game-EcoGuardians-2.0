using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float gravityMultiplier;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float jumpHorizontalSpeed;

    [SerializeField]
    private Transform cameraTransform;

    private CharacterController characterController;
    private Animator animator;

    private float originalStepOffset;
    private float ySpeed;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;
    private bool wasGrounded;
    public bool isInGame;
    private float inputMagnitude;
    private AudioSource playerAudioSource;

    private float walkSFXInterval = 0.5f;
    private float runSFXInterval = 0.3f;
    private float nextSFXTime;
    private SurfaceDetector surfaceDetector;
    private float lastFootstepTime;

    public bool GetIsGrounded() { return isGrounded; }

    void Start()
    {
        isInGame = false;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        nextSFXTime = 0f;
        playerAudioSource = GetComponent<AudioSource>();
        wasGrounded = true;
        surfaceDetector = gameObject.AddComponent<SurfaceDetector>();
        lastFootstepTime = 0f;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection;

        if (isInGame)
        {
            jumpHeight = 1f;
            movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        }
        else
        {
            jumpHeight = 0f;
            inputMagnitude = 0f;
            movementDirection = Vector3.zero;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (!isRunning)
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05F, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;

        if (isJumping && ySpeed > 0 && !Input.GetButton("Jump"))
        {
            gravity *= 2;
        }

        ySpeed += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        // Detect jump start
        if (Input.GetButtonDown("Jump") && isInGame)
        {
            jumpButtonPressedTime = Time.time;
            if (isGrounded)
            {
                OnJumpStart();
            }
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                animator.SetBool("isJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
                OnJumpStart();
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isGrounded", false);
            isGrounded = false;
            if ((isJumping && ySpeed < 0) || (ySpeed < -10f))
            {
                animator.SetBool("isFalling", true);
            }
        }

        // Check surface less frequently
        if (Time.time - lastFootstepTime >= 0.5f)
        {
            surfaceDetector.CheckSurface();
            lastFootstepTime = Time.time;
        }

        if (movementDirection != Vector3.zero && isGrounded)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);

            if (Time.time >= nextSFXTime)
            {
                AudioManager.Instance.PlayFootstepSound(isRunning);
                nextSFXTime = Time.time + (isRunning ? runSFXInterval : walkSFXInterval);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            nextSFXTime = 0f;
        }

        if (!isGrounded)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

        // Detect landing
        if (!wasGrounded && isGrounded)
        {
            OnLand();
        }

        wasGrounded = isGrounded;
    }

    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity = AdjustVelocityToSlope(velocity);
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private void OnJumpStart()
    {
        AudioManager.Instance.PlayJumpSound();
    }

    private void OnLand()
    {
        AudioManager.Instance.PlayLandSound();
    }
}