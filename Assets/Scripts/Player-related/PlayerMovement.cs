using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity; // tracks vertical speed (falling/jumping)
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<Health>();
    }

    [Header("Passive - Double Jump")]
    public int maxJumps = 2;
    private int jumpsUsed = 0;

    [Header("Fall Damage")]
    public float fallDamageThreshold = -10f;
    public float fallDamageMultiplier = 2f;
    private bool wasGroundedLastFrame = true;
    private float velocityYLastFrame = 0f;
    private Health playerHealth;

void Update()
{
    isGrounded = controller.isGrounded;

    if (isGrounded && !wasGroundedLastFrame && velocityYLastFrame < fallDamageThreshold)
{
    if (ItemEffectSystem.Instance == null || !ItemEffectSystem.Instance.HasRavenFeather())
    {
        float excess = fallDamageThreshold - velocityYLastFrame;
        int fallDamage = Mathf.RoundToInt(excess * fallDamageMultiplier);
        playerHealth?.TakeDamage(fallDamage);
    }
}

    isGrounded = controller.isGrounded;

    if (isGrounded && velocity.y < 0)
    {
        velocity.y = -2f;
        jumpsUsed = 0; // reset available jumps whenever grounded
    }

    float inputX = Input.GetAxis("Horizontal");
    float inputZ = Input.GetAxis("Vertical");
    Vector3 move = transform.right * inputX + transform.forward * inputZ;

    bool isSprinting = Input.GetKey(KeyCode.LeftShift);
    float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
    controller.Move(move * currentSpeed * Time.deltaTime);
    if (ItemEffectSystem.Instance != null)
    currentSpeed *= ItemEffectSystem.Instance.GetFoxShrineMoveSpeedMultiplier(transform.position);

    if (Input.GetButtonDown("Jump") && jumpsUsed < maxJumps)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpsUsed++;
    }


    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);

    wasGroundedLastFrame = isGrounded;
    velocityYLastFrame = velocity.y;
    
}

public bool IsSprinting() => Input.GetKey(KeyCode.LeftShift);
public bool IsGrounded() => isGrounded;

    
}