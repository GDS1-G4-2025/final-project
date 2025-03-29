using System.Collections.Generic;
using UnityEngine;
/*
Task Data will be attached to the object that acts as the final step in a task. 
This will be where points are allocated through. Any task that acts as a
requirement or pre-requisite to completing the main task will not include this
*/
public class TaskData : MonoBehaviour
{
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private List<GameObject> _collidingPlayers;
    [SerializeField] private List<GameObject> _nodes;
    public void AddNode(GameObject node){ _nodes.Add(node); }
    public List<GameObject> GetNodes(){ return _nodes;}
    [SerializeField] private int _pointAllocation;
    public int GetPointAllocation(){ return _pointAllocation; }

    [SerializeField] private GameObject _tryComplete;
    public GameObject GetTryComplete(){ return _tryComplete;}
    public void SetTryComplete(GameObject value){ _tryComplete = value; }


    void Start()
    {
        _taskManager.AddTask(this.gameObject);
        _collidingPlayers = new List<GameObject>();
        this.gameObject.SetActive(false);
    }

    public void TaskCompleted(){
        _taskManager.CompleteTask(this.gameObject, GetTryComplete());
    }

    void OnEnable()
    {
        foreach(GameObject node in _nodes){ node.SetActive(true); }
    }

    void OnDisable()
    {
        foreach(GameObject node in _nodes){ node.SetActive(false); }
        foreach(GameObject player in _collidingPlayers){ player.GetComponent<PlayerData>().SetCollidingTask(null); }
    }

        private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            if(!_collidingPlayers.Contains(other.gameObject)){ 
                other.gameObject.GetComponent<PlayerData>().SetCollidingTask(this.gameObject);
                _collidingPlayers.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            if(_collidingPlayers.Contains(other.gameObject)) { 
                other.gameObject.GetComponent<PlayerData>().SetCollidingTask(null);
                _collidingPlayers.Remove(other.gameObject);
            }
        }
    }
}
