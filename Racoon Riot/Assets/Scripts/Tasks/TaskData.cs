using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/*
Task Data will be attached to the object that acts as the final step in a task.
This will be where points are allocated through. Any task that acts as a
requirement or pre-requisite to completing the main task will not include this
*/
public class TaskData : MonoBehaviour
{
    private bool _isInteractable;
    public bool IsInteractable{
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }
    public string taskName;
    public int pointAllocation;
    public Player playerAttempting;

    [SerializeField] private List<GameObject> _nodes;
    public void AddNode(GameObject node) { _nodes.Add(node); }
    public IReadOnlyList<GameObject> Nodes { get => _nodes; }

    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private List<GameObject> _collidingPlayers;


    private void Start()
    {
        _taskManager = FindAnyObjectByType<TaskManager>();
        _taskManager.AddTask(this);
        _collidingPlayers = new List<GameObject>();
    }

    public void TaskCompleted()
    {
        _taskManager.CompleteTask(this, playerAttempting);
    }

    private void OnEnable()
    {
        foreach (var node in _nodes) { node.SetActive(true); }
    }

    private void OnDisable()
    {
        foreach (var node in _nodes) { node.SetActive(false); }
        foreach (var player in _collidingPlayers) { player.GetComponent<Player>().collidingTask = null; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_collidingPlayers.Contains(other.gameObject))
            {
                other.gameObject.GetComponent<Player>().collidingTask = this;
                _collidingPlayers.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(_collidingPlayers.Count > 0)
            {
                if (_collidingPlayers.Contains(other.gameObject))
                {
                    other.gameObject.GetComponent<Player>().collidingTask = null;
                    _collidingPlayers.Remove(other.gameObject);
                }
            }
            if (playerAttempting != null) 
            {  
                if(playerAttempting.gameObject == other.gameObject)
                {
                    playerAttempting = null;
                }
            }
        }
    }
}
