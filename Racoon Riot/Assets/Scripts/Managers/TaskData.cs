using UnityEngine;

public class TaskData : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private int _pointAllocation;
    public int GetPointAllocation(){ return _pointAllocation; }


    void Start()
    {
        _taskManager.AddTask(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
