using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SimultaneousTransmitter : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    public void OnMap(TaskData taskData)
    {
        taskData.RootTask.GetComponent<SimultaneousReceiver>().AddTransmitter(this);
    }

    public void AttemptTask()
    {
        _taskData.Complete = true;
        _taskData.RootTask.GetComponent<SimultaneousReceiver>().CheckLocked();
    }

    public void AttemptTaskCancel()
    {
        _taskData.Complete = false;
    }
}
