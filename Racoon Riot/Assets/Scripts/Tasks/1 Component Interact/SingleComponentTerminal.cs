using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    public bool AttemptTask()
    {
        _taskData.CompleteTask();
        return true;
    }
}
