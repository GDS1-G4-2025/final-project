using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;
    private InputAction _interactAction;
    private PlayerInput _playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions.FindAction("Interact");
        if (_interactAction != null)
        {
            _interactAction.performed += ctx => PickUpObject();
        }
    }

    void OnDestroy ()
    {
        if (_interactAction != null)
        {
            _interactAction.performed -= ctx => PickUpObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Object != null && Object.transform.parent == racoonHand.transform)
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
