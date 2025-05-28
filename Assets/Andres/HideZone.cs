using UnityEngine;

public class HideZone : MonoBehaviour
{
    [SerializeField] Transform hideCamTransform; // Asigna el objeto vacío aquí
    private bool playerInZone = false;
    private bool isHidden = false;
    private GameObject player;
    private FPCamMovement playerCamera;
    private GameObject floatingInteractionCanvas;

    void Start()
    {
        floatingInteractionCanvas = gameObject.GetComponentInChildren<InteractionFloatingText>(true).gameObject;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform.parent.gameObject;
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
        if (floatingInteractionCanvas) floatingInteractionCanvas.SetActive(!isHidden);

        if (playerCamera != null)
        {
            playerCamera.setPositionToFollow(isHidden ? hideCamTransform : playerCamera.getOriginalCamTarget());
        }
        PlayerMovement playerMovementScript = player.GetComponent<PlayerMovement>();
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = !isHidden;
            player.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = !isHidden;
            //player.GetComponent<PlayerTaskManager>().completeTask("hide_zone");

            // Notificar al sistema de UI
            UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();

            if (uiNotificationManager != null && GameManager.Instance.hideCount==0)
            {
                GameManager.Instance.hideCount += 1;
                uiNotificationManager.NotifyEventOnGame("¡Misión Completada!");
            }
        }

    }
}
