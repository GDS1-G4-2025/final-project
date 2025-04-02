using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : Interactable
{
    protected Rigidbody _rb;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        _rb.isKinematic = true;
    }
}
