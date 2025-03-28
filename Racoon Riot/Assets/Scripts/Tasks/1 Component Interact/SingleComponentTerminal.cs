using System.Collections.Generic;
using UnityEngine;

public class SingleComponentTerminal : MonoBehaviour
{
    [SerializeField] private TaskData _taskData;
    [SerializeField] private List<GameObject> _collidingPlayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _taskData = this.gameObject.GetComponent<TaskData>();
        _collidingPlayers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            else{ Debug.Log("ID does not exist?"); }
        }
    }
}
