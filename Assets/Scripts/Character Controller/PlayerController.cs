using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float jumpButtonGracePeriod = 0.2f;
    [SerializeField] private float jumpHorizontalSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

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

    void Start()
    {
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

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (!isInGame)
        {
            jumpHeight = 0f;
            inputMagnitude = 0f;
            movementDirection = Vector3.zero;
        }
        else
        {
            jumpHeight = 1f;
            inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
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

        ySpeed += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

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

        if (Time.time - lastSurfaceCheckTime >= 0.5f)
        {
            surfaceDetector.CheckSurface();
            lastSurfaceCheckTime = Time.time;
        }

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        Vector3 velocity;
        if (!isGrounded)
        {
            velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);
        }

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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJumpSound();
        }
    }

    private void OnLand()
    {
        Debug.Log("Landed with speed: " + ySpeed);
        if (ySpeed < 0f && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLandSound();
        }
        ySpeed = 0f; // Reset ySpeed when landing
    }

    public void setCameraTransform(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    public bool GetIsGrounded() { return isGrounded; }
}