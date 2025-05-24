using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    private float distanceWalked = 0f;
    [SerializeField] private float targetDistance = 100f;

    private Vector3 lastPosition;
    private bool taskCompleted = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (taskCompleted) gameObject.GetComponent<DistanceTracker>().enabled = false;

        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        distanceWalked += distanceThisFrame;
        lastPosition = transform.position;

        //Debug.Log("Distancia: " + distanceWalked);

        if (distanceWalked >= targetDistance)
        {
            taskCompleted = true;
            // Notificar al sistema de UI
            UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();
            if (uiNotificationManager != null) uiNotificationManager.NotifyEventOnGame("¡Haz recorrido 10 metros!");

            // Completar la misión
            TaskManager taskManager = FindObjectOfType<TaskManager>();
            if (taskManager != null) taskManager.CompleteTask("walk_20m");
        }
    }
}
