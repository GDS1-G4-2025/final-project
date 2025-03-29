using UnityEngine;

public class SimultaneousReceiver : MonoBehaviour
{
    [SerializeField] private bool _locked = true;
    [SerializeField] private TaskData _taskData;
    [SerializeField] private int _nodesActive = 0;

    void Start()
    {
        _taskData = this.gameObject.GetComponent<TaskData>();
    }

    public void AdjustNodeActive(int difference){
        _nodesActive += difference;
        if(_nodesActive == _taskData.GetNodes().Count){ _locked = false; }
    }

    void FixedUpdate()
    {
        if(_taskData.GetTryComplete() != null && !_locked){
            _taskData.TaskCompleted();
        }
    }
}
