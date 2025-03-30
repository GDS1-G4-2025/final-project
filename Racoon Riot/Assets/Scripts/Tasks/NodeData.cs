using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] private bool _interactableNode;
    [SerializeField] private List<GameObject> _collidingPlayers;
    [SerializeField] private GameObject _parentTask;
    public GameObject GetParentTask(){ return _parentTask; }

    private bool _activateNode;
    public bool GetActivateNode(){ return _activateNode; }
    public void SetActivateNode(bool activateNode){ _activateNode = activateNode; }

    void Start()
    {
        _parentTask.GetComponent<TaskData>().AddNode(this.gameObject);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            if(!_collidingPlayers.Contains(other.gameObject)){ 
                if(_interactableNode && other.gameObject.GetComponent<PlayerData>().GetCollidingNode() == null){
                    other.gameObject.GetComponent<PlayerData>().SetColldingNode(this.gameObject);
                }
                _collidingPlayers.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            if(_collidingPlayers.Contains(other.gameObject)) { 
                if(_interactableNode && other.gameObject.GetComponent<PlayerData>().GetCollidingNode() == this.gameObject){
                    other.gameObject.GetComponent<PlayerData>().SetColldingNode(null);
                }
                _collidingPlayers.Remove(other.gameObject);
            }
        }
        if(_collidingPlayers.Count == 0){ _activateNode = false;}
    }
}
