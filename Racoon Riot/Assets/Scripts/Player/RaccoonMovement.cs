using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RaccoonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 10.0f;
    [SerializeField] private float _rotationSpeed = 150.0f;
    [SerializeField] private float _accel = 10.0f;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpDelay = 1.8f;

    //Components
    private Rigidbody _rb;
    private Animator _animator;

    //Player Movement
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _movementInput;

    //Jumping
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _canJump = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _jumpAction = _playerInput.actions.FindAction("Jump");
    }

    private void OnEnable()
    {
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _jumpAction.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _jumpAction.performed -= OnJumpPerformed;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        _jumpPressed = true;
    }

    //Movement updates
    private void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();

        //Adds more responsive feel to jumping
        ApplyStrongerFalling();

        UpdateAnimatorParameters();

        float inputMagnitude = _movementInput.magnitude;
        //Debug.Log($"InputMagnitude = {inputMagnitude}");
    }

    private void CheckGrounded()
    {
        if (_groundCheck == null)
        {
            _isGrounded = true;
            return;
        }

        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }

    //Move
    private void HandleMovement()
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

        //Lerping for smoother movement
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _accel * Time.fixedDeltaTime);
    }

    //Jump
    private void HandleJump()
    {
        if (_jumpPressed && _isGrounded && _canJump)
        {
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = 0f;
            _rb.linearVelocity = velocity;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            _animator.SetTrigger("jumpTrigger");
            StartCoroutine(JumpCooldown());
        }
        _jumpPressed = false;
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(_jumpDelay);
        _canJump = true;
    }

    //Adds more responsive feel to jumping
    private void ApplyStrongerFalling()
    {
        if (!_isGrounded && _rb.linearVelocity.y < 0f)
        {
            Vector3 extraGravity = Physics.gravity * _fallMultiplier - Physics.gravity;
            _rb.AddForce(extraGravity, ForceMode.Acceleration);
        }
    }

    //Checking where ground check is being performed in scene view
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

    private void UpdateAnimatorParameters()
    {
        float inputMagnitude = _movementInput.magnitude;
        float inputX = _movementInput.x;
        float inputY = _movementInput.y;
        float speed = _rb.linearVelocity.magnitude;

        _animator.SetFloat("inputMagnitude", inputMagnitude);
        _animator.SetFloat("inputX", inputX);
        _animator.SetFloat("inputY", inputY);
        _animator.SetFloat("speed", speed);
        _animator.SetFloat("verticalVelocity", _rb.linearVelocity.y);
        
        if (_isGrounded)
        {
            _animator.SetBool("isGrounded", true);
        }
        else
        {
            _animator.SetBool("isGrounded", false);
        }
    }
}