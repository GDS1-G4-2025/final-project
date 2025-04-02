using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Throwable : Pickupable
{
    [SerializeField] private float _throwForce = 10f;

    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }

    public void Throw(Vector3 direction)
    {
        transform.SetParent(null); // Detach from player hand
        _col.enabled = true;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddForce(direction * _throwForce, ForceMode.Impulse);
        Debug.Log(name + " was thrown!");
    }
}
