using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
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

        // Notificar al sistema de UI
        UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();
        if (uiNotificationManager != null)
        {
            uiNotificationManager.NotifyEventOnGame(uiText);
        }

        // Si existe audio, esperar mientras reproduce
        if (audioSource != null)
        {
            audioSource.Play();

            // Desactivar visuales mientras suena el audio
            GetComponent<Collider>().enabled = false;
            if (TryGetComponent<MeshRenderer>(out var renderer))
                renderer.enabled = false;

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
