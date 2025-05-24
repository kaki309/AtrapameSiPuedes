using UnityEngine;

public class InteractionFloatingText : MonoBehaviour
{
    private Transform cameraToLookAt;
    [SerializeField] private bool scaleWithDistance = true;
    [SerializeField] private float scaleFactor = 0.05f;

    private void Start()
    {
        if (Camera.main != null)
            cameraToLookAt = Camera.main.transform;

        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (cameraToLookAt == null) return;

        // Hacer que el canvas mire al jugador
        transform.rotation = Quaternion.LookRotation(transform.position - cameraToLookAt.position);

        // Escalar proporcionalmente a la distancia
        if (scaleWithDistance)
        {
            float distance = Vector3.Distance(transform.position, cameraToLookAt.position);
            float adjustedScale = distance * scaleFactor;
            transform.localScale = Vector3.one * adjustedScale;
        }
    }
}
