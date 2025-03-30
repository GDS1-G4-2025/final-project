using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int _numberOfConcurrentTasks;
    [SerializeField] private List<GameObject> _upcomingTasks;
    [SerializeField] private List<GameObject> _currentTasks;
    public List<GameObject> GetCurrentTasks(){ return _currentTasks; }
    [SerializeField] private List<GameObject> _completedTasks;

    public void FixedUpdate(){
        if(_currentTasks.Count < _numberOfConcurrentTasks && _upcomingTasks.Count > 0){
            MoveTaskToCurrent(_upcomingTasks[Random.Range(0, _upcomingTasks.Count-1)]);
        }
    }

    public void AddTask(GameObject task){
        _upcomingTasks.Add(task);
    }

    public void MoveTaskToCurrent(GameObject task){
        _currentTasks.Add(task);
        _upcomingTasks.Remove(task);
        task.SetActive(true);
    }

    public void CompleteTask(GameObject task, GameObject player){
        _completedTasks.Add(task);
        _currentTasks.Remove(task);
        player.GetComponent<PlayerData>().AddPoints(task.GetComponent<TaskData>().GetPointAllocation());
        task.SetActive(false);
    }
}
