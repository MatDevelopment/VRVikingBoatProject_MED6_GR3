using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInfo : MonoBehaviour
{
    [TextArea(3,20)]
    [SerializeField] private string task;
    [TextArea(3,20)]
    [SerializeField] private string subTasks;
    
    public string GetPrompt()
    {
        return $"Current task: {task}\n" +
               $"Subtasks to complete: {subTasks}\n";
    }
}
