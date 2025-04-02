using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int _numberOfConcurrentTasks;
    [SerializeField] private List<GameObject> _upcomingTasks;
    [SerializeField] private List<GameObject> _completedTasks;

    public List<GameObject> CurrentTasks { get; } = new List<GameObject>();

    public void Update()
    {
        if (CurrentTasks.Count < _numberOfConcurrentTasks && _upcomingTasks.Count > 0)
        {
            MoveTaskToCurrent(_upcomingTasks[Random.Range(0, _upcomingTasks.Count )]);
        }
    }

    public void AddTask(GameObject task)
    {
        _upcomingTasks.Add(task);
    }

    private void MoveTaskToCurrent(GameObject task)
    {
        CurrentTasks.Add(task);
        _upcomingTasks.Remove(task);
        task.GetComponent<TaskData>().BeginTask();
    }

    public void CompleteTask(GameObject task)
    {
        _completedTasks.Add(task);
        CurrentTasks.Remove(task);
        task.GetComponent<TaskData>().Complete = false;
    }

    public void OnStepComplete(TaskData taskRoot, TaskData taskData)
    {
        taskRoot.stepName = taskData.RootTask.GetComponent<TaskData>().taskName ?? taskRoot.GetComponent<TaskData>().taskName;
    }
}
