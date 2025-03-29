using UnityEngine;

public class ThrowableItem : PickableItem
{
    public float throwForce = 10f;

    public void Throw(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            transform.SetParent(null); // Detach from player hand
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
            Debug.Log(itemName + " was thrown!");
        }
    }
}