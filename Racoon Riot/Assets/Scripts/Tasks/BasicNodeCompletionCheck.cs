using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class BasicNodeCompletionCheck : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;

    public void Start()
    {
        _taskData = GetComponent<TaskData>();
    }
    
    public void OnActivate()
    {
        _taskData.CompleteTask(null);
    }
}
