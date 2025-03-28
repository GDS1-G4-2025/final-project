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
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteract;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        _isInteracting = true;
    }

        void FixedUpdate() 
    {
        if (_isInteracting && _playerData.GetCollidingTask() != null){
        //if(Input.GetKeyDown(KeyCode.V) && _playerData.GetCollidingTask() != null){
            Debug.Log("1");
            _playerData.GetCollidingTask().GetComponent<TaskData>().SetTryComplete(this.gameObject);
        }
    }
}
