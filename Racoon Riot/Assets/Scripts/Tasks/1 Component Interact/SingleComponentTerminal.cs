using System.Collections.Generic;
using UnityEngine;

public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    void Start()
    {
        _taskData = this.gameObject.GetComponent<TaskData>();

    }

    void FixedUpdate()
    {
        if(_taskData.GetTryComplete() != null){
            _taskData.TaskCompleted();
        }
    }
}
