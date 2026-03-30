using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public Rigidbody rgbody;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;

    [Header("Control Triggers")]
    [SerializeField] bool Forward;
    [SerializeField] bool Left;
    [SerializeField] bool Backward;
    [SerializeField] bool Right;
    [SerializeField] bool OnGround;
    [SerializeField] float groundCheckDistance = 1;
    [SerializeField] LayerMask GroundMask;

    // Improved jump variables
    [Header("Jump Improvements")]
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("Properties")]
    [SerializeField] bool ThirdPerson;

    private float lastJumpTime;
    private float lastGroundedTime;
    private float lastJumpInputTime;
    private bool jumpConsumed;
    private float ogspeed;
    public static PlayerMovement3D Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        ogspeed = speed;
    }   

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) Forward = true;
        else Forward = false;
        if (Input.GetKey(KeyCode.A)) Left = true;
        else Left = false;
        if (Input.GetKey(KeyCode.S)) Backward = true;
        else Backward = false;
        if (Input.GetKey(KeyCode.D)) Right = true;
        else Right = false;

        // Improved jump input handling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpInputTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed * 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = ogspeed;
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        Vector3 movement = Vector3.zero;

        if (Forward) movement.z += 1;
        if (Backward) movement.z -= 1;
        if (Right) movement.x += 1;
        if (Left) movement.x -= 1;

        if (CanJump())
        {
            ExecuteJump();
        }

        if (movement != Vector3.zero)
        {
            movement = movement.normalized;

            Vector3 cameraRelativeMovement = Camera.main.transform.TransformDirection(movement);
            cameraRelativeMovement.y = 0;
            cameraRelativeMovement = cameraRelativeMovement.normalized;

            rgbody.AddForceAtPosition(cameraRelativeMovement * speed, transform.position);

            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMovement);
            if (ThirdPerson) gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 7 * Time.deltaTime);
        }

        if (!ThirdPerson)
        {
            Quaternion cameraRotation = Camera.main.transform.rotation;
            cameraRotation.x = 0;
            cameraRotation.z = 0;
            gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, 10 * Time.deltaTime);
        }
    }

    void CheckGrounded()
    {
        bool wasGrounded = OnGround;

        // Keep your original raycast logic
        Vector3 rayOrigin = transform.position + (Vector3.up * groundCheckDistance);
        OnGround = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance + groundCheckDistance, GroundMask);

        // Update coyote time
        if (OnGround)
        {
            lastGroundedTime = Time.time;
            jumpConsumed = false; // Reset jump consumption when grounded
        }

        // Visual debug
        Debug.DrawRay(rayOrigin, Vector3.down * (groundCheckDistance + groundCheckDistance), OnGround ? Color.green : Color.red);
    }

    private bool CanJump()
    {
        // Check cooldown
        if (Time.time - lastJumpTime < jumpCooldown)
            return false;

        // Check if jump input is within buffer time AND we have coyote time OR are grounded
        bool hasValidJumpInput = Time.time - lastJumpInputTime <= jumpBufferTime;
        bool canJumpFromGround = OnGround || (Time.time - lastGroundedTime <= coyoteTime);

        return hasValidJumpInput && canJumpFromGround && !jumpConsumed;
    }

    private void ExecuteJump()
    {
        // Reset vertical velocity for consistent jump height
        Vector3 velocity = rgbody.linearVelocity;
        velocity.y = 0;
        rgbody.linearVelocity = velocity;

        // Apply jump force
        rgbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Reset jump state
        lastJumpTime = Time.time;
        lastJumpInputTime = 0; // Consume the jump input
        jumpConsumed = true;
        OnGround = false;
    }
}