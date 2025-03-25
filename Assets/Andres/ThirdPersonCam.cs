using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] Transform player, playerObj, orientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position. x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(inputDir != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime*rotationSpeed);
        }
    }
}
