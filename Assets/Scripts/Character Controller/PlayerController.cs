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

    private SurfaceDetector surfaceDetector;
    private float lastSurfaceCheckTime;

    public bool IsRunning { get; private set; }

    public bool GetIsGrounded() { return isGrounded; }

    void Start()
    {
        isInGame = false;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        wasGrounded = true;
        surfaceDetector = gameObject.AddComponent<SurfaceDetector>();
        lastSurfaceCheckTime = 0f;
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

        IsRunning = Input.GetKey(KeyCode.LeftShift);
        if (!IsRunning)
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

            // Only reset ySpeed to a small value when the player is grounded and not jumping
            if (!isJumping)
            {
                ySpeed = -0.5f; // Slight downward force to keep grounded
            }

            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -2 * gravity); // Apply proper jump force
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

        // Always apply gravity when the player is not grounded
        if (!characterController.isGrounded)
        {
            ySpeed += gravity * Time.deltaTime;
        }

        // Check surface less frequently
        if (Time.time - lastSurfaceCheckTime >= 0.5f)
        {
            surfaceDetector.CheckSurface();
            lastSurfaceCheckTime = Time.time;
        }

        if (movementDirection != Vector3.zero && isGrounded)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
        else
        {
            animator.SetBool("isMoving", false);
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
        Debug.Log("Jump");
    }

    private void OnLand()
    {
        Debug.Log(ySpeed);

        if (ySpeed < -2f)
        {
            Debug.Log("Fall");
            AudioManager.Instance.PlayLandSound();
            Debug.Log("Land");
        }
    }
}
