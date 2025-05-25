using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTaskManager : MonoBehaviour
{
    private TaskManager taskManager;

    void Start()
    {
        taskManager = FindObjectOfType <TaskManager>();
    }
    public void completeTask(string taskId)
    {
        if (taskManager != null) taskManager.CompleteTask(taskId);
    }
}
