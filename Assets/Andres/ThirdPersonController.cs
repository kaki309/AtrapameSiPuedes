using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class ThirdPersonController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform orientation;

    //Movement variables
    [SerializeField] float moveSpeed, jumpForce;
    Vector2 moveInput;

    // Controls inputs
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        //Variables initialization
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        //Suscription to events
        jumpAction.started += jump;
        jumpAction.canceled -= jump;
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
       move();
    }
    void move(){
        Vector3 moveDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    void jump(InputAction.CallbackContext context){
        rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
    }
}
