using UnityEngine;

public class HideZoneTask : MonoBehaviour
{
    private bool isHidden = false;
    private bool taskCompleted = false;

    void Start()
    {
        isHidden = false;
        taskCompleted = false;
    }
    void Update()
    {
        if (taskCompleted) gameObject.GetComponent<HideZoneTask>().enabled = false;

        if (isHidden)
        {
            Debug.Log("tarea escondido");
            // Completar la misión
            TaskManager taskManager = FindObjectOfType<TaskManager>();
            if (taskManager != null) taskManager.CompleteTask("hide_zone");

            taskCompleted = true;
        }

    }

    public void completeTask()
    {
        isHidden = true;
    }
}
