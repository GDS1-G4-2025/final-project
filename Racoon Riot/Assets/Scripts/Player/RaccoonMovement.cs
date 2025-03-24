using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RaccoonMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10.0f;
    [SerializeField] private float _rotationSpeed = 0.0001f;
    [SerializeField] private float _accel = 15f;

    private Rigidbody _rb;
    private Animator _animator;

    //Player Movement
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Vector2 _movementInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {

        _movementInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        //Desired direction based on camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //Ignore vertical
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = (cameraForward * _movementInput.y) +
                                       (cameraRight * _movementInput.x);
        desiredMoveDirection.Normalize();

        //Rotate to face direction of travel
        if (desiredMoveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime
            );
        }

        //Move
        Vector3 targetVelocity = desiredMoveDirection * _movementSpeed;
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _accel * Time.fixedDeltaTime);
    }
}
