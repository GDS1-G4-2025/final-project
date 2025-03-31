using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Throwable : Pickupable
{
    [SerializeField] private float _throwForce = 10f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction)
    {
        transform.SetParent(null); // Detach from player hand
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddForce(direction * _throwForce, ForceMode.Impulse);
        Debug.Log(name + " was thrown!");
    }
}
