using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    public int playersRequired, triesRequired;
    private int currentTries;

    private void Start()
    {
        _taskData = gameObject.GetComponent<TaskData>();
    }

    public void AttemptTask(List<Player> players)
    {
        currentTries += 1;
        if (currentTries >= triesRequired && players.Count >= playersRequired)
        {
            _taskData.CompleteTask(players);
        }
    }
}
