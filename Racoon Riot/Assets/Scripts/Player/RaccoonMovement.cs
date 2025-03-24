using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RaccoonMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10.0f;
    [SerializeField] private float _rotationSpeed = 50.0f;
    [SerializeField] private float _accel = 10.0f;

    private Rigidbody _rb;
    private Animator _animator;

    // Player Movement
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
        //Rotate
        float turnInput = _movementInput.x;
        float forwardInput = _movementInput.y;

        if (Mathf.Abs(turnInput) > 0.1f)
        {
            float turnDegrees = turnInput * _rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, turnDegrees, 0f);
        }

        Vector3 targetVelocity = transform.forward * forwardInput * _movementSpeed;
        targetVelocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _accel * Time.fixedDeltaTime);
    }
}