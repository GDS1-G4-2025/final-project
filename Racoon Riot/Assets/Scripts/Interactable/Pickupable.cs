using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : Interactable
{
    protected Rigidbody _rb;
    protected Collider _col;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log(name + " has been picked up!");
    }

    public void AttachTo(GameObject parent)
    {
        transform.SetParent(parent.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
        if (_col != null)
        {
            _col.enabled = false;
        }
    }

    public void Drop(Transform dropperTransform, float placeDistance)
    {
        // Detach from parent
        transform.SetParent(null);

        // Calculate target position
        Vector3 dropperPosition = dropperTransform.position;
        Vector3 dropperForward = dropperTransform.forward;
        // Calculate position in front, preserving the player's Y level in the initial calculation
        Vector3 targetPosition = dropperPosition + dropperForward.normalized * placeDistance;

        transform.position = targetPosition;
        transform.rotation = Quaternion.identity;

        if (_col != null)
        {
            _col.enabled = true;
        }
        else
        {
            Debug.LogError($"{name} cannot enable collider - missing Collider.", this);
        }

        if (_rb != null)
        {
            _rb.isKinematic = false; // Allow physics interaction
            _rb.useGravity = true;   // Enable gravity

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogError($"{name} cannot be dropped properly - missing Rigidbody.", this);
        }

        Debug.Log($"{name} placed at {targetPosition}");
    }
}