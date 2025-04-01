using System.Collections.Generic;
using UnityEngine;

public class NodeData : MonoBehaviour
{
    private bool _isInteractable;
    public bool IsInteractable{
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }
    [SerializeField] private bool _isPickupable;
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
        _isInteractable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_collidingPlayers.Contains(other.gameObject))
            {
                if (_isPickupable && other.gameObject.TryGetComponent(out Player player))
                {
                    if (player.collidingNode == null)
                        player.collidingNode = this;
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
                if (_isPickupable && other.gameObject.TryGetComponent(out Player player))
                {
                    if (player.collidingNode == this)
                        player.collidingNode = null;
                }
                _collidingPlayers.Remove(other.gameObject);
            }
        }
        if (_collidingPlayers.Count == 0) { Active = false; }
    }
}
