using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject powerupNotification;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private Image notificationIcon;
    
    [Header("Powerup Icons")]
    [SerializeField] private Sprite carrotIcon;
    // Puedes añadir más iconos para futuros powerups aquí
    
    [Header("Spawn Settings")]
    [SerializeField] private GameObject energyCarrotPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minSpawnTime = 30f;
    [SerializeField] private float maxSpawnTime = 60f;
    
    private void Start()
    {
        // Asegurarse de que la notificación está desactivada al inicio
        if (powerupNotification != null)
            powerupNotification.SetActive(false);
            
        // Iniciar rutina de generación de zanahorias si hay puntos de spawn
        if (spawnPoints != null && spawnPoints.Length > 0 && energyCarrotPrefab != null)
        {
            StartCoroutine(SpawnPowerups());
        }
    }
    
    // Método público para notificar que se ha recogido un powerup
    public void NotifyPowerupCollected(string powerupName, Sprite icon = null)
    {
        if (powerupNotification != null && notificationText != null)
        {
            // Configurar texto e icono
            notificationText.text = powerupName;
            
            if (icon != null && notificationIcon != null)
                notificationIcon.sprite = icon;
                
            // Mostrar notificación
            StopCoroutine(nameof(ShowNotificationCoroutine));
            StartCoroutine(ShowNotificationCoroutine());
        }
    }
    
    // Corrutina para mostrar la notificación por unos segundos
    private IEnumerator ShowNotificationCoroutine()
    {
        powerupNotification.SetActive(true);
        
        // Efecto de fade in
        CanvasGroup canvasGroup = powerupNotification.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            float duration = 0.5f;
            float elapsed = 0;
            
            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1;
        }
        
        // Mantener visible
        yield return new WaitForSeconds(2.5f);
        
        // Efecto de fade out
        if (canvasGroup != null)
        {
            float duration = 0.5f;
            float elapsed = 0;
            
            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        powerupNotification.SetActive(false);
    }
    
    // Corrutina para generar zanahorias periodicamente
    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            
            // Seleccionar un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // Verificar que no haya ya una zanahoria en ese punto
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 1f);
            bool canSpawn = true;
            
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<EnergyCarrot>() != null)
                {
                    canSpawn = false;
                    break;
                }
            }
            
            // Generar la zanahoria si es posible
            if (canSpawn)
            {
                Instantiate(energyCarrotPrefab, spawnPoint.position, Quaternion.identity);
                //Debug.Log("¡Zanahoria energética generada!");
            }
        }
    }
}