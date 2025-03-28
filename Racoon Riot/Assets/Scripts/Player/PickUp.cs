using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;
    public float throwForce = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
    public void PickUpObject(){
        Object.transform.SetParent(racoonHand.transform);
        Object.transform.localPosition = Vector3.zero;

        
    }
    public void ThrowObject()
    {
        Object.transform.SetParent(null);
        Rigidbody rb = Object.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.isKinematic = false;
            rb.AddForce (racoonHand.transform.forward * throwForce, ForceMode.Impulse);
        } 
        Object = null;

    }
    public void OnTriggerEnter(Collider other)
    {
        if(Object == null && other.CompareTag("PickUpObject"))
        {
            Object = other.gameObject;
            Debug.Log (other.gameObject.name + " is picked up!");

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PickUpObject") && Object == other.gameObject)
        {
            Object = null;

        }
    }
}
