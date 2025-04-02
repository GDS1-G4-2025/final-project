using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldObject : MonoBehaviour
{
    [SerializeField] public Pickupable heldObject;

    [SerializeField] private Pickupable _pickUpTarget;
    [SerializeField] private GameObject _itemHolder; // Child object of player to hold items

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }

    public void PickUpObject(InputAction.CallbackContext ctx)
    {
        if (_pickUpTarget == null) return;
        if (IsHoldingObject()) return;

        Debug.Log("picking up");

        heldObject = _pickUpTarget;
        heldObject.AttachTo(_itemHolder);
        heldObject.GetComponent<Collider>().enabled = false;
        _pickUpTarget = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsHoldingObject() && other.gameObject.TryGetComponent(out Pickupable pickupable))
        {
            _pickUpTarget = pickupable;
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
