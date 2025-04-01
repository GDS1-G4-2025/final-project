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
                    _player.collidingTask.playerAttempting = _player;
                }

                // Handle node interaction
                if (_player.collidingNode != null)
                {
                    _player.collidingNode.Active = true;
                }
                break;
            }
            // Handle release/end interaction
            case InputActionPhase.Canceled:
            {
                // Handle task interaction
                if (_player.collidingTask != null)
                {
                    _player.collidingTask.playerAttempting = null;
                }

                // Handle node interaction
                if (_player.collidingNode != null)
                {
                    _player.collidingNode.Active = false;
                }
                break;
            }
        }
    }
}
