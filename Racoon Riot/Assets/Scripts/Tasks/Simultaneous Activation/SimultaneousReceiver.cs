using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class SimultaneousReceiver : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    [SerializeField] private List<SimultaneousTransmitter> _transmitters;

    private void Start()
    {
        _taskData = GetComponent<TaskData>();
    }

    public void AddTransmitter(SimultaneousTransmitter transmitter)
    {
        _transmitters.Add(transmitter);
    }

    public void CheckLocked()
    {
        foreach (SimultaneousTransmitter transmitter in _transmitters)
        {
            if(!transmitter.GetComponent<TaskData>().Complete){ return; }
        }
        foreach (SimultaneousTransmitter transmitter in _transmitters)
        {
            transmitter.GetComponent<TaskData>().CompleteTask(null);
        }
    }

    public void AttemptTask(List<Player> players)
    {
        if(players.Count < _transmitters.Count){ return; }
        _taskData.CompleteTask(players);
    }
}
