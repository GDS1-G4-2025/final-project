using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    // Should move this somewhere else to avoid the circular reference
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            // Handle press/begin interaction
            case InputActionPhase.Started:
            {
                // Handle task interaction
                if (_player.collidingTask != null)
                {
                    if(_player.hold.heldObject == null)
                    {
                        _player.collidingTask.playersAttempting.Add(_player);
                        _player.collidingTask.PlayerAttempt(_player);
                    }
                    else if(_player.hold.heldObject.gameObject.TryGetComponent<TaskData>(out TaskData taskData))
                    {
                        if(taskData.RootTask == _player.collidingTask.gameObject)
                        {
                            _player.collidingTask.playersAttempting.Add(_player);
                            _player.collidingTask.PlayerAttempt(_player);
                        }
                    }
                }
                break;
            }
            // Handle release/end interaction
            case InputActionPhase.Canceled:
            {
                // Handle task interaction
                if (_player.collidingTask != null)
                {
                    if(_player.collidingTask.playersAttempting.Contains(_player))
                    {
                        _player.collidingTask.playersAttempting.Remove(_player);
                        _player.collidingTask.PlayerAttemptCancel(_player);
                    }
                }
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<TaskData>(out TaskData taskData))
        {
            if(taskData.Active && !taskData.collidingPlayers.Contains(_player))
            {
                _player.collidingTask = taskData;
                taskData.collidingPlayers.Add(_player);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<TaskData>(out TaskData taskData))
        {
            _player.collidingTask = null;
            if(taskData.collidingPlayers.Contains(_player)) 
            { 
                taskData.collidingPlayers.Remove(_player);
            }
            if(taskData.playersAttempting.Contains(_player))
            {
                taskData.PlayerAttemptCancel(_player);
                taskData.playersAttempting.Remove(_player);
            }
        }
    }
}
