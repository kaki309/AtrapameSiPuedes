using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("UI Notification")]
    [SerializeField] bool notifyUI;
    [SerializeField] private string uiText;
    private bool playerInZone = false;
    private bool isCollected = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInZone && !isCollected && Input.GetKeyDown(KeyCode.F))
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;

        if (notifyUI)
        {
            // Notificar al sistema de UI
            UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();
            if (uiNotificationManager != null)
            {
                uiNotificationManager.NotifyEventOnGame(uiText);
            }
        }

        // Completar la misión
        TaskManager taskManager = FindObjectOfType<TaskManager>();
        if (taskManager != null) taskManager.CompleteTask("steal_carrot");

        // Desactivar visuales
        GetComponent<Collider>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        // Si existe audio, esperar mientras reproduce
        if (audioSource != null)
        {
            audioSource.Play();
            // Destruir el objeto después de que termine el sonido
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            // Si no existe audio, destruir objeto directamente
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}
