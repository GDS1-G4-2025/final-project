using UnityEngine;

public class PayloadReceiver : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    [SerializeField] private int _receivedCount;
    void Start()
    {
        _taskData = this.gameObject.GetComponent<TaskData>();
    }

    void FixedUpdate()
    {
        if(_taskData.GetTryComplete() != null){
            foreach(GameObject node in _taskData.GetNodes()){
                if(node == _taskData.GetTryComplete().GetComponent<PlayerData>().GetHeldObject()){
                    _receivedCount += 1;
                    node.transform.parent = this.transform;
                    node.SetActive(false);
                    _taskData.GetTryComplete().GetComponent<PlayerData>().SetHeldObject(null);
                }
            }
        }
        if(_receivedCount >= _taskData.GetNodes().Count){ _taskData.TaskCompleted(); }
    }
}
