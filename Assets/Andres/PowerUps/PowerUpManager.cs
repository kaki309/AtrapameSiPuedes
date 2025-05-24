using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
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
        // Iniciar rutina de generación de zanahorias si hay puntos de spawn
        if (spawnPoints != null && spawnPoints.Length > 0 && energyCarrotPrefab != null)
        {
            StartCoroutine(SpawnPowerups());
        }
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