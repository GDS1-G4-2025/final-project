using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerPickupThrow : MonoBehaviour
{
    [SerializeField] public Pickupable heldObject;
    
    [SerializeField] private Pickupable _pickUpTarget;
    [SerializeField] private GameObject _itemHolder; // Child object of player to hold items

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public void OnObjectInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log($"OnObjectInteract called: phase={ctx.phase}, performed={ctx.performed}");
        if (!ctx.performed) return;

        // If I'm already holding something, throw it, else pick up.
        if (IsHoldingObject())
        {
            AttemptThrow();
        }
        else
        {
            AttemptPickup();
        }
    }

    private void AttemptPickup()
    {
        if (_pickUpTarget == null) return;
        if (IsHoldingObject()) return; // Just a safeguard

        Debug.Log("picking up");
        heldObject = _pickUpTarget;
        heldObject.AttachTo(_itemHolder);
        heldObject.GetComponent<Collider>().enabled = false;
        _pickUpTarget = null;
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
