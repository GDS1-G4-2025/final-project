using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;

    [SerializeField] private float throwForce = 10f;
    
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && Object != null)
        {
            PickUpObject();
        }
        if (Object != null && Object.transform.parent == racoonHand.transform)
        {
            Object.transform.position = racoonHand.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.T) && Object != null)
        {
            ThrowObject();
        }
        
    }

    public void ThrowObject()
    {
        _pickUpTarget.transform.SetParent(null);
        Rigidbody rb = _pickUpTarget.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.isKinematic = false;
            rb.AddForce (_handItemHandler.transform.forward * throwForce, ForceMode.Impulse);
        } 
        _pickUpTarget = null;
        _handItemHandler = null;
    }

    public void PickUpObject(InputAction.CallbackContext ctx){
        Debug.Log("picking up");
        if(_pickUpTarget != null){
            _pickUpTarget.transform.SetParent(_heldItemHandler.transform);
            _pickUpTarget.transform.localPosition = Vector3.zero;
            this.gameObject.GetComponent<PlayerData>().SetHeldObject(_pickUpTarget);
            _pickUpTarget = null;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(Object == null && other.CompareTag("PickUpObject"))
        {
            _pickUpTarget = this.gameObject.GetComponent<PlayerData>().GetHeldItem();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PickUpObject") && Object == other.gameObject)
        {
            _pickUpTarget = null;
        }
    }
}
