using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    private void FixedUpdate()
    {
        if(_taskData.playerAttempting){
            _taskData.TaskCompleted();
        }
    }
}
