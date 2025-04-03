using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerDestruction : MonoBehaviour
{
    [SerializeField] private Destructible _destructionTarget;
    [SerializeField] private bool _enableSnapping = true;

    private PlayerSnapping _playerSnapping;

    private void Awake()
    {
        _playerSnapping = GetComponent<PlayerSnapping>();
    }

    public void OnDestruction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (_destructionTarget != null && (_playerSnapping == null || !_playerSnapping.IsSnapping))
            {
                // Decide whether to snap or destroy immediately
                if (_enableSnapping && _playerSnapping != null)
                {
                    Destructible targetToDestroy = _destructionTarget;

                    _playerSnapping.StartSnap(
                        targetToDestroy.transform,
                        () =>
                        {
                            // Check if the originally targeted object still exists and is the correct one
                            if (targetToDestroy != null && targetToDestroy.gameObject.activeInHierarchy)
                            {
                                targetToDestroy.Destroy();
                            }
                            else
                            {
                                Debug.Log("Target for destruction was lost or destroyed before snap completed.");
                            }
                        }
                    );
                }
                else
                {
                    // Destroy Immediately
                    Debug.Log($"Destroying {_destructionTarget.name} (Snapping disabled or unavailable).");
                    _destructionTarget.Destroy();
                }
            }
            else if (ctx.phase == InputActionPhase.Started && _destructionTarget != null && _playerSnapping != null && _playerSnapping.IsSnapping)
            {
                Debug.Log("Destruction input ignored: Player is currently snapping.");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Destructible destructible))
        {
            _destructionTarget = destructible;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_destructionTarget != null && _destructionTarget.gameObject == other.gameObject)
        {
            _destructionTarget = null;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Destructible destructible))
        {
            _destructionTarget = destructible;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (_destructionTarget != null && _destructionTarget.gameObject == other.gameObject)
        {
            _destructionTarget = null;
        }
    }
}
