using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerData _playerData;
    private PlayerInput _playerInput;
    private InputAction _interactAction;
    private bool _isInteracting;
    public bool IsInteracting(){ return _isInteracting; }

        private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions.FindAction("Interact");
    }

        private void OnEnable()
    {
        _interactAction.performed += OnInteract;
        _interactAction.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteract;
        _interactAction.canceled -= OnCanceled;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        _isInteracting = true;
    }
    public void OnCanceled(InputAction.CallbackContext ctx)
    {
        _isInteracting = false;
    }

        void FixedUpdate() 
    {
        if (_isInteracting && _playerData.GetCollidingTask() != null){
            _playerData.GetCollidingTask().GetComponent<TaskData>().SetTryComplete(this.gameObject);
        }
    }
}
