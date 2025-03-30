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
            this.gameObject.GetComponent<PlayerData>().SetHeldObject(_pickUpTarget);
            _pickUpTarget.GetComponent<Rigidbody>().isKinematic = true;
            _pickUpTarget = null;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.GetComponent<PlayerData>().GetHeldObject() == null && other.CompareTag("PickUpObject"))
        {
            _pickUpTarget = other.gameObject;
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
