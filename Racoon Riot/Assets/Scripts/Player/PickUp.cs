using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public GameObject Object;
    public GameObject racoonHand;
    public float throwForce = 10f;
    [SerializeField] private GameObject _pickUpTarget;
    [SerializeField] private GameObject _heldItemHandler;
    private InputAction _interactAction;
    private PlayerInput _playerInput;
    private InputAction _throwAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions.FindAction("Interact");
        _throwAction = _playerInput.actions.FindAction("Throw");
    }

    void OnEnable()
    {
        _interactAction.performed += PickUpObject;
        _throwAction.performed += ThrowObject;
    }

    void OnDisable()
    {
        _interactAction.performed -= PickUpObject;
        _throwAction.performed -= ThrowObject;
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
    public void ThrowObject(InputAction.CallbackContext ctx)
    {
        if (_pickUpTarget != null)
        {
            ThrowableItem throwable = _pickUpTarget.GetComponent<ThrowableItem>();
            if (throwable != null)
            {
                Vector3 throwDirection = transform.forward;
                throwable.Throw(throwDirection);
                _pickUpTarget = null;
            }
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






