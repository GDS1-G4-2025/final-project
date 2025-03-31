using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SimultaneousReceiver : MonoBehaviour
{
    [SerializeField] private bool _locked = true;
    [SerializeField] private TaskData _taskData;
    [SerializeField] private int _activeNodes;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    public void AdjustActiveNodes(int difference){
        _activeNodes += difference;
        if(_activeNodes == _taskData.Nodes.Count){ _locked = false; }
    }

    private void FixedUpdate()
    {
        if(_taskData.playerAttempting && !_locked){
            _taskData.TaskCompleted();
        }
    }
}
