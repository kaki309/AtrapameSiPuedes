using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskData> tasks = new List<TaskData>();

    public UITaskListManager uiManager;

    public void CompleteTask(string taskId)
    {
        TaskData task = tasks.Find(t => t.id == taskId);
        if (task != null && !task.isCompleted)
        {
            task.isCompleted = true;
            //Debug.Log($"Tarea completada: {task.description}");
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateTaskList(tasks);
        }
    }


    private void Start()
    {
        UpdateUI(); // Inicializa la UI con las tareas pendientes
    }
}
