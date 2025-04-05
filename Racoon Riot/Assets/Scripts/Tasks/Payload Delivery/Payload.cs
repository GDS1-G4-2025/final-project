using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class Payload : MonoBehaviour
{
    private TaskData _taskData;
    private void Start(){ _taskData = GetComponent<TaskData>(); }

    public void OnMap(TaskData taskData)
    {
        taskData.RootTask.GetComponent<PayloadReceiver>().AddPayload(this);
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void OnActivate()
    { 
        _taskData.CompleteTask(null); 
    }
}
