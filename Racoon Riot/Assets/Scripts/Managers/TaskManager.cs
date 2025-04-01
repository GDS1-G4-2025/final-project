using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int _numberOfConcurrentTasks;
    [SerializeField] private List<TaskData> _upcomingTasks;
    [SerializeField] private List<TaskData> _completedTasks;

    public List<TaskData> CurrentTasks { get; } = new List<TaskData>();

    public void Update()
    {
        if (CurrentTasks.Count < _numberOfConcurrentTasks && _upcomingTasks.Count > 0)
        {
            MoveTaskToCurrent(_upcomingTasks[Random.Range(0, _upcomingTasks.Count - 1)]);
        }
    }

    public void AddTask(TaskData task)
    {
        _upcomingTasks.Add(task);
    }

    private void MoveTaskToCurrent(TaskData task)
    {
        CurrentTasks.Add(task);
        _upcomingTasks.Remove(task);
        task.gameObject.SetActive(true);
    }

    public void CompleteTask(TaskData task, Player player)
    {
        _completedTasks.Add(task);
        CurrentTasks.Remove(task);
        player.score.AddPoints(task.pointAllocation);
        task.gameObject.SetActive(false);
    }
}
