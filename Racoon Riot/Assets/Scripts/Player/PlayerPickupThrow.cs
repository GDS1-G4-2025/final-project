using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerPickupThrow : MonoBehaviour   
{
    [SerializeField] public Pickupable heldObject;
    
    [SerializeField] private Pickupable _pickUpTarget;
    [SerializeField] private GameObject _itemHolder; // Child object of player to hold items

    //Interaction
    private bool _canInteract = true;
    [SerializeField] private float _interactCooldown = 0.3f; // Time between interactions

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public void OnObjectPickupDrop(InputAction.CallbackContext ctx)
    {
        Debug.Log($"OnObjectInteract called: phase={ctx.phase}, performed={ctx.performed}");

        if (ctx.phase == InputActionPhase.Started)
        {
            if (!_canInteract)
            {
                Debug.Log("Interact attempt blocked by cooldown.");
                return;
            }

            Debug.Log("Processing Interact Input (Started phase)");

            _canInteract = false;

            if (IsHoldingObject())
            {
                AttemptDrop();
            }
            else
            {
                AttemptPickup();
            }

            StartCoroutine(InteractCooldownTimer());
        }
    }

    private IEnumerator InteractCooldownTimer()
    {
        yield return new WaitForSeconds(_interactCooldown);
        _canInteract = true;
    }

    public void OnObjectThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            AttemptThrow();
        }
    }

    private void AttemptPickup()
    {
        if (_pickUpTarget == null)
        {
            Debug.Log("Attempted pickup but no target.");
            return;
        }
        if (IsHoldingObject())
        {
            Debug.Log("Attempted pickup but already holding object.");
            return; // Safeguard
        }

        Debug.Log("Picking up: " + _pickUpTarget.name);
        heldObject = _pickUpTarget;
        heldObject.AttachTo(_itemHolder);
        _pickUpTarget = null;
    }

    private void AttemptDrop()
    {
        if (heldObject == null) return;

        Debug.Log("Dropping: " + heldObject.name);
        Pickupable objectToDrop = heldObject;
        heldObject = null;

        float placementDistance = 1.0f;
        objectToDrop.Drop(transform, placementDistance);

        _pickUpTarget = null; // Prevent immediate re-pickup
    }

    private void AttemptThrow()
    {
        if (heldObject == null) return;
        if (heldObject.TryGetComponent<Throwable>(out Throwable throwable))
        {
            Debug.Log("throwing");
            throwable.Throw(transform.forward);
            heldObject = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsHoldingObject() && other.gameObject.TryGetComponent(out Pickupable pickupable))
        {
            _pickUpTarget = pickupable;
            Debug.Log("Pickup target set");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_pickUpTarget != null && _pickUpTarget.gameObject == other.gameObject)
        {
            _pickUpTarget = null;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!IsHoldingObject() && other.gameObject.TryGetComponent(out Pickupable pickupable))
        {
            _pickUpTarget = pickupable;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (_pickUpTarget != null && _pickUpTarget.gameObject == other.gameObject)
        {
            _pickUpTarget = null;
        }
    }
}
