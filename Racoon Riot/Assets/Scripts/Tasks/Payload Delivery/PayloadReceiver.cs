using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class PayloadReceiver : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    [SerializeField] private int _receivedCount;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    private void FixedUpdate()
    {
        if(_taskData.playerAttempting){
            foreach(var node in _taskData.Nodes){
                if(node == _taskData.playerAttempting.hold.heldObject?.gameObject){
                    _receivedCount += 1;
                    node.transform.parent = transform;
                    node.SetActive(false);
                    _taskData.playerAttempting.hold.heldObject = null;
                }
            }
        }
        if(_receivedCount >= _taskData.Nodes.Count){ _taskData.TaskCompleted(); }
    }
}
