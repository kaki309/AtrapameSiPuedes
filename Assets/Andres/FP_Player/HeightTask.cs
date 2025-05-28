using UnityEngine;

public class HeightTask : MonoBehaviour
{
    public float heightThreshold = 15f;
    private bool taskCompleted = false;

    void Update()
    {
        if (taskCompleted) gameObject.GetComponent<HeightTask>().enabled = false;

        if (gameObject.transform.position.y >= heightThreshold)
        {
            taskCompleted = true;

            // Notificar al sistema de UI
            UINotificationManager uiNotificationManager = FindObjectOfType<UINotificationManager>();
            //if (uiNotificationManager != null) uiNotificationManager.NotifyEventOnGame("¡Wow, estás a 3 metros del suelo!");

            if (uiNotificationManager != null)
            {
                uiNotificationManager.QueueNotification("¡Wow, estás a 3 metros del suelo!");
                uiNotificationManager.QueueNotification("¡Misión Completada!");
            }


            // Completar la misión
            TaskManager taskManager = FindObjectOfType<TaskManager>();
            if (taskManager != null) taskManager.CompleteTask("3m_high");

            //Debug.Log("Haz alcanzado 3 metros de altura.");
        }
    }
}
