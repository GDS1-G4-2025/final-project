using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class Payload : MonoBehaviour
{
    private TaskData _taskData;
    private void Start(){ _taskData = GetComponent<TaskData>(); }
    public void OnActivate(){ _taskData.CompleteTask(); }
}
