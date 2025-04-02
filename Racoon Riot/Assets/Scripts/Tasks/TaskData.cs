using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskData : MonoBehaviour
{
    public string taskName; //Name of the task, appears on task list
    public string stepName;
    public int pointAllocation; //Points allocated per task. This can be applied on any task at any level
    [SerializeField] private GameObject _rootTask; //Parent Task or Task Manager
    public GameObject RootTask 
    { 
        get { return _rootTask; } 
    }
    [SerializeField] private List<GameObject> _nodeTasks; //Child tasks, the very beginning of a task will have no NodeTasks
    public IReadOnlyList<GameObject> NodeTasks 
    { 
        get { return _nodeTasks; } 
    }

    public List<Player> collidingPlayers; //A list of the players currently colliding with the task
    public List<Player> playersAttempting; //A list of the players currently attempting the task

    [SerializeField] private bool _isActive; //Is this task segment active? all prerequisite tasks completed
    public bool Active
    {
        get { return _isActive; }
        set 
        { 
            _isActive = value; 
            if(_isActive)
            {
                OnStepComplete(this);
                /*
                This section will contain functions attributed to unique task types
                to be run as soon as the task is activated.

                Example:
                Payload will automatically complete on activation as there's no task
                until PayloadReceiver
                */
                if(TryGetComponent<Payload>(out Payload taskHandle)){ taskHandle.OnActivate();}
            }
            else
            {
                foreach(Player player in collidingPlayers)
                {
                    player.collidingTask = null;
                }
                collidingPlayers.Clear();
                playersAttempting.Clear();
            }
        }
    }
    [SerializeField] private bool _isComplete; //Has this task been completed? Checked in parent task to determine if it can activate
    public bool Complete
    { 
        get { return _isComplete; }
        set { _isComplete = value; }
    }
    
    void Awake()
    {
        if(transform.parent.parent == null)
        {
            MapTasks(transform.parent.gameObject);
            RootTask.GetComponent<TaskManager>().AddTask(this.gameObject);
        }
        Active = false;
    }

    public GameObject MapTasks(GameObject root) //Only called on Awake, Recursive code to map out all root/node tasks
    {
        _rootTask = root;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.TryGetComponent<TaskData>(out TaskData taskData))
            {
                _nodeTasks.Add(taskData.MapTasks(this.gameObject));
            }
        }
        /*
        This section can be used for extra mapping functionality, like mapping payloads that won't stay parented.
        */
        GetComponent<Payload>()?.OnMap(this);
        GetComponent<SimultaneousTransmitter>()?.OnMap(this);
        return this.gameObject;
    }

    public void OnStepComplete(TaskData taskData)
    {
        if(RootTask.TryGetComponent<TaskManager>(out TaskManager taskManager))
        {
            taskManager.OnStepComplete(this, taskData);
        }
        else
        {
            RootTask.GetComponent<TaskData>().OnStepComplete(taskData);
        }
    }

    public void BeginTask() //Called when a task first moves into current task list. Finds the lowest children and begins there
    {
        if(NodeTasks.Count > 0)
        {
            Active = false;
            foreach(GameObject node in NodeTasks)
            {
                node.GetComponent<TaskData>().BeginTask();
            }
        }
        else{
            Active = true;
        }
    }

    public void TryActivateTask() //Checks if all nodes are complete, then activates task and resets nodes
    {
        foreach(GameObject node in _nodeTasks)
        {
            if(node.TryGetComponent<TaskData>(out TaskData nodeData))
            { 
                if(!nodeData.Complete){ return; }
            }
        }
        foreach(GameObject node in _nodeTasks)
        {
            if(node.TryGetComponent<TaskData>(out TaskData nodeData))
            { 
                nodeData.Complete = false;
            }
        }
        Active = true;
    }

    public bool PlayerAttempt(Player player)
    {
        GetComponent<SingleComponentTerminal>()?.AttemptTask(playersAttempting);
        GetComponent<PayloadReceiver>()?.AttemptTask(playersAttempting);
        GetComponent<SimultaneousTransmitter>()?.AttemptTask();
        GetComponent<SimultaneousReceiver>()?.AttemptTask(playersAttempting);
        //PERFORM TASK GOES HERE
        return false;
    }

    public bool PlayerAttemptCancel(Player player)
    {
        if(player == null || !playersAttempting.Contains(player)){ return false; }
        playersAttempting.Remove(player);

        //CANCEL TASK GOES HERE
        GetComponent<SimultaneousTransmitter>()?.AttemptTaskCancel();
        return true;
    }

    public void CompleteTask(List<Player> completingPlayers) //Awards points, resets task, and moves to next task up
    {
        _isActive = false;
        _isComplete = true;
        if(RootTask.transform.parent != null)
        {
            RootTask.GetComponent<TaskData>().TryActivateTask();
        }
        else
        {
            RootTask.GetComponent<TaskManager>().CompleteTask(this.gameObject);
        }
        if(completingPlayers != null)
        {
            foreach(Player player in completingPlayers)
            {
                player.score.AddPoints(pointAllocation);
            }
        }
        collidingPlayers.Clear();
        playersAttempting.Clear();
    }
}

//Future Use Resetting  payloadReceiver
/*
    if(TryGetComponent<PayloadReceiver>(out PayloadReceiver payloadReceiver))
    {
        foreach(GameObject node in NodeTasks)
        {
            node.GetComponent<Payload>()?.OnMap(node.GetComponent<TaskData>());
        }
    }
*/