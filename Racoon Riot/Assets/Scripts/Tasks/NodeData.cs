using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] private bool _isInteractable;
    [SerializeField] private List<GameObject> _collidingPlayers;
    [SerializeField] public TaskData parentTask;

    private bool _active;
    public bool Active
    {
        get => _active;
        set
        {
            if (_active == value) return;
            _active = value;

            // Notify transmitter about the state change
            GetComponent<SimultaneousTransmitter>()?.OnNodeStateChanged(_active);
        }
    }

    private void Start()
    {
        parentTask.AddNode(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_collidingPlayers.Contains(other.gameObject))
            {
                if (_isInteractable && other.gameObject.GetComponent<Player>().collidingNode == null)
                {
                    other.gameObject.GetComponent<Player>().collidingNode = this;
                }
                _collidingPlayers.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_collidingPlayers.Contains(other.gameObject))
            {
                if (_isInteractable && other.gameObject.GetComponent<Player>().collidingNode == this)
                {
                    other.gameObject.GetComponent<Player>().collidingNode = null;
                }
                _collidingPlayers.Remove(other.gameObject);
            }
        }
        if (_collidingPlayers.Count == 0) { Active = false; }
    }
}
