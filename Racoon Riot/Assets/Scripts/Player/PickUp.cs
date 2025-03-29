using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpTarget;
    [SerializeField] private GameObject _heldItemHandler;
    private InputAction _interactAction;
    private PlayerInput _playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions.FindAction("Interact");
    }

    void OnEnable()
    {
        _interactAction.performed += PickUpObject;
    }

    void OnDisable()
    {
        _interactAction.performed -= PickUpObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_pickUpTarget != null && _pickUpTarget.transform.parent == _heldItemHandler.transform)
        {
            _pickUpTarget.transform.position = _heldItemHandler.transform.position;
        }
        
    }
    public void PickUpObject(InputAction.CallbackContext ctx){
        Debug.Log("picking up");
        if(_pickUpTarget != null){
            _pickUpTarget.transform.SetParent(_heldItemHandler.transform);
            _pickUpTarget.transform.localPosition = Vector3.zero;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(_pickUpTarget == null && other.CompareTag("PickUpObject"))
        {
            _pickUpTarget = other.gameObject;
            Debug.Log (other.gameObject.name + " is picked up!");

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PickUpObject") && _pickUpTarget == other.gameObject)
        {
            _pickUpTarget = null;

        }
    }
}
