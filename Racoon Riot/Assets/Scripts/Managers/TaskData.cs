using UnityEngine;

public class TaskData : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private int _pointAllocation;
    public int GetPointAllocation(){ return _pointAllocation; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _taskManager.AddTask(this.gameObject);
    }
}
