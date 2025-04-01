using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskData : MonoBehaviour
{
    public string taskName;
    public int pointAllocation;
    [SerializeField] private GameObject _rootTask;
    public GameObject RootTask 
    { 
        get { return _rootTask; } 
    }
    [SerializeField] private List<GameObject> _nodeTasks;
    public IReadOnlyList<GameObject> NodeTasks 
    { 
        get { return _nodeTasks; } 
    }

    public List<Player> collidingPlayers;
    public List<Player> playersAttempting;

    [SerializeField] private bool _isActive;
    public bool Active
    {
        get { return _isActive; }
        set 
        { 
            _isActive = value; 
            if(_isActive)
            {
                //OnActivate Tasks
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
    [SerializeField] private bool _isComplete;
    public bool Complete
    { 
        get { return _isComplete; }
        set { _isComplete = value; }
    }
    
    void Start()
    {
        if(transform.parent.parent == null)
        {
            MapTasks(transform.parent.gameObject);
            RootTask.GetComponent<TaskManager>().AddTask(this.gameObject);
        }
        Active = false;
    }

    public GameObject MapTasks(GameObject root)
    {
        _rootTask = root;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.TryGetComponent<TaskData>(out TaskData taskData))
            {
                _nodeTasks.Add(taskData.MapTasks(this.gameObject));
            }
        }
        return this.gameObject;
    }

    public void BeginTask()
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

    public void TryActivateTask()
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

    public void CompleteTask(List<Player> completingPlayers)
    {
        _isActive = false;
        _isComplete = true;
        if(RootTask.transform.parent != null)
        {
            RootTask.GetComponent<TaskData>().TryActivateTask();
        }
        else
        {
            RootTask.GetComponent<TaskManager>().CompleteTask(this.gameObject, completingPlayers);
        }
        collidingPlayers.Clear();
        playersAttempting.Clear();
    }
}