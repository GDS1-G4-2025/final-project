using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Throwable : Pickupable
{
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private float _upwardThrowFactor = 0.5f;

    public void Throw(Vector3 direction)
    {
        transform.SetParent(null); // Detach from player hand

        if (_col != null)
        {
            _col.enabled = true;
        }
        else
        {
            Debug.LogError($"{name} cannot throw - missing Collider.", this);
        }

        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            Vector3 directionNorm = direction.normalized;
            Vector3 combinedDirection = directionNorm + (Vector3.up * _upwardThrowFactor); // Upward force component
            Vector3 throwDirection = combinedDirection.normalized; // Normalize the final direction

            _rb.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError($"{name} cannot throw - missing Rigidbody.", this);
            return; // Can't apply force without a rigidbody
        }

        Debug.Log($"{name} was thrown with force: {direction.normalized * _throwForce}");
    }
}