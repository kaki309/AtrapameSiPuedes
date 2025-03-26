using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement: MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform orientation;

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

    void Start()
    {
        //Variables initialization
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check Ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight/2 + 0.2f, whatIsGround);
        //Debug.DrawRay(transform.position, Vector3.down * (playerHeight/2 + 0.2f), Color.red);

        // Floor friction
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag=0;
        
        // Limit Speed
        speedControl();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) jump();
    }
    void FixedUpdate()
    {
       move();
    }
    void move(){
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }
    void speedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    void jump(){
        
        if (readyToJump && isGrounded){
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            readyToJump = false;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }
    void resetJump(){
        readyToJump = true;
    }
}
