using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] Transform player, playerObj, orientation;
    [SerializeField] InputActionReference moveAction;
    Vector2 moveDir;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position. x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        moveDir = moveAction.action.ReadValue<Vector2>();
        Vector3 inputDir = orientation.forward * moveDir.y + orientation.right * moveDir.x;
        
        if(inputDir != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime*rotationSpeed);
        }
    }
}
