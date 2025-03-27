using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;
    private bool isInPickupRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isInPickupRange && Input.GetKeyDown(KeyCode.E) && Object != null)
        {
            PickUpObject();
        }
        if (Object != null)
        {
            Object.transform.position = racoonHand.transform.position;
        }
        
    }
    public void PickUpObject(){
        Object.transform.SetParent(racoonHand.transform);
        Object.transform.localPosition = Vector3.zero;

        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PickUpObject"))
        {
            isInPickupRange = true;
            Object = other.gameObject;
            Debug.Log (other.gameObject.name + " is picked up!");

        }
    }
    public void OnTriggerExit(Collider other)
    {
        isInPickupRange = false;
        if(other.CompareTag("PickUpObject") && Object == other.gameObject)
        {
            Object = null;

        }
    }
}
