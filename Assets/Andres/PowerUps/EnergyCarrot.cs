using UnityEngine;

public class EnergyCarrot : MonoBehaviour
{
    [Header("Power-up Settings")]
    [SerializeField] private float energyRestoreAmount = 100f; // Cantidad de energía a restaurar (100% por defecto)
    [SerializeField] private float rotationSpeed = 50f; // Velocidad de rotación para efecto visual
    [SerializeField] private float bobSpeed = 1f; // Velocidad de movimiento vertical
    [SerializeField] private float bobHeight = 0.3f; // Altura del movimiento vertical
    [SerializeField] private ParticleSystem collectEffect; // Efecto de partículas al recoger
    [SerializeField] private AudioClip collectSound; // Sonido al recoger
    [SerializeField] private string uiText;

    private Vector3 startPosition;
    private float bobTime;

    private void Start()
    {
        startPosition = transform.position;
        bobTime = 0f;

        collectSound = this.GetComponent<AudioClip>();
    }

    private void Update()
    {
        // Rotación continua para efecto visual
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Efecto de movimiento vertical (bob)
        bobTime += Time.deltaTime * bobSpeed;
        float newY = startPosition.y + Mathf.Sin(bobTime) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador colisiona con la zanahoria
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            if (playerMovement != null)
            {
                // Restaurar energía del jugador
                playerMovement.RestoreEnergy(energyRestoreAmount);

                // Notificación en la UI

                UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();
                if (uiNotificationManager != null)
                {
                    uiNotificationManager.NotifyEventOnGame(uiText);
                }

                // Reproducir efectos
                PlayCollectEffects();

                // Destruir la zanahoria
                Destroy(gameObject);
                //Debug.Log("Zanahoria destruida");
            }
        }
    }

    private void PlayCollectEffects()
    {
        // Reproducir efecto de partículas
        if (collectEffect != null)
        {
            ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            effect.Play();

            // Destruir el sistema de partículas cuando termine
            Destroy(effect.gameObject, effect.main.duration + 0.5f);
        }

        // Reproducir sonido
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }
}