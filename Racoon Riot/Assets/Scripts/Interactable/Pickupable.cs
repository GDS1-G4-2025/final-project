using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : Interactable
{
    private Rigidbody _rb;

    private void Awake()
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
