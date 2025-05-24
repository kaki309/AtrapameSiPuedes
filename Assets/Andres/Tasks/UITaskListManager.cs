using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITaskListManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    public void UpdateTaskList(List<TaskData> allTasks)
    {
        taskText.text = "";
        foreach (TaskData task in allTasks)
        {
            string formatted = task.isCompleted
                ? $"<s>• {task.description}</s>"
                : $"• {task.description}";

            taskText.text += formatted + "\n";
        }
    }
}
