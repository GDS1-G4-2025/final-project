using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    public void AttemptTask(List<Player> players)
    {
        _taskData.CompleteTask(players);
    }
}
