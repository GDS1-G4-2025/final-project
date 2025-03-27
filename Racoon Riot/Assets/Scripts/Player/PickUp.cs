using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Object != null)
        {
            Object.transform.position = racoonHand.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.E) && Object != null)
        {
            PickUpObject();
        }
        
    }
    public void PickUpObject(){
        Object.transform.SetParent(racoonHand.transform);
        Object.transform.localPosition = new Vector3 (0f,0f,0f);
        // Object.transform.localScale = new Vector3 (1f,1f,1f);

        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PickUpObject"))
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
