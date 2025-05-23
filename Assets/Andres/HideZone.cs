using UnityEngine;

public class HideZone : MonoBehaviour
{
    public Transform hideCamTransform; // Asigna el objeto vacío aquí
    private bool playerInZone = false;
    private bool isHidden = false;
    private GameObject player;
    private PlayerMovement playerMovementScript; // Referencia al script que quieres desactivar

    private FPCamMovement playerCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform.parent.gameObject;
            playerMovementScript = player.GetComponent<PlayerMovement>();
            playerCamera = GameObject.FindObjectOfType<FPCamMovement>();
            playerInZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (isHidden)
            {
                ToggleHide(); // Automáticamente salir al dejar la zona
            }
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.Q))
        {
            ToggleHide();
        }
    }

    void ToggleHide()
    {
        isHidden = !isHidden;
        player.SetActive(!isHidden); // Desactivar o activar el jugador

        if (playerCamera != null)
        {
            playerCamera.setPositionToFollow(isHidden ? hideCamTransform : playerCamera.getOriginalCamTarget());
        }
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = !isHidden;
        }

    }
}
