using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerDestruction : MonoBehaviour
{
    [SerializeField] private Destructable _destructionTarget;

    public void OnDestruction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (_destructionTarget != null)
            {
                _destructionTarget.Destroy();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Destructable destructable))
        {
            _destructionTarget = destructable;
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
        if (other.gameObject.TryGetComponent(out Destructable destructable))
        {
            _destructionTarget = destructable;
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
