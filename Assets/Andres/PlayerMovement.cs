using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform orientation;
    public TMP_Text texto;

    [Header("Movement")]
    float horizontalInput;
    float verticalInput;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    bool isGrounded;
    bool readyToJump = true;

    [Header("Super Jump")]
    [SerializeField] float superJumpMultiplier = 2f;
    [SerializeField] float superJumpDuration = 5f;
    [SerializeField] float superJumpCooldown = 20f;
    bool canActivateSuperJump = false;
    bool isSuperJumpActive = false;
    float superJumpTimer = 0f;
    float superJumpCooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.2f, whatIsGround);

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        speedControl();

        if (Input.GetKeyDown(KeyCode.Space)) jump();

        // Powers
        HandleSuperJump();

        // Debug
        texto.text = $"Tiempo cooldown:{superJumpCooldownTimer}\nTiempo activo:{superJumpTimer}";
    }

    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void jump()
    {
        if (readyToJump && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            readyToJump = false;
            float currentJumpForce = isSuperJumpActive ? jumpForce * superJumpMultiplier : jumpForce;
            rb.AddForce(transform.up * currentJumpForce, ForceMode.Impulse);
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    void resetJump()
    {
        readyToJump = true;
    }

    void HandleSuperJump()
    {
        if (!canActivateSuperJump)
        {
            superJumpCooldownTimer += Time.deltaTime;
            if (superJumpCooldownTimer >= superJumpCooldown)
            {
                canActivateSuperJump = true;
                superJumpCooldownTimer = 0f;
            }
        }

        if (canActivateSuperJump && Input.GetKeyDown(KeyCode.E))
        {
            ActivateSuperJump();
        }

        if (isSuperJumpActive)
        {
            superJumpTimer += Time.deltaTime;
            if (superJumpTimer >= superJumpDuration)
            {
                DeactivateSuperJump();
            }
        }
    }

    void ActivateSuperJump()
    {
        isSuperJumpActive = true;
        canActivateSuperJump = false;
        superJumpTimer = 0f;
    }

    void DeactivateSuperJump()
    {
        isSuperJumpActive = false;
    }
}
