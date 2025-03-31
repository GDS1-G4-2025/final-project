using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private static readonly int InputMagnitude = Animator.StringToHash("inputMagnitude");
    private static readonly int InputX = Animator.StringToHash("inputX");
    private static readonly int InputY = Animator.StringToHash("inputY");
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int VerticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int JumpTrigger = Animator.StringToHash("jumpTrigger");

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
    [SerializeField] private float _jumpDelay = 1.0f;

    //Components
    private Rigidbody _rb;
    private Animator _animator;

    //Player Movement
    private Vector2 _movementInput;

    //Jumping
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _canJump = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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

        //Debug.Log($"InputMagnitude = {_movementInput.magnitude}");
    }

    private void CheckGrounded()
    {
        if (!_groundCheck)
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
        var turnInput = _movementInput.x;
        var forwardInput = _movementInput.y;

        if (Mathf.Abs(turnInput) > 0.1f)
        {
            var turnDegrees = turnInput * _rotationSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, turnDegrees, 0f);
        }

        var targetVelocity = transform.forward * (forwardInput * _movementSpeed);
        targetVelocity.y = _rb.linearVelocity.y;

        //Lerping for smoother movement
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _accel * Time.fixedDeltaTime);
    }

    //Jump
    private void HandleJump()
    {
        if (_jumpPressed && _isGrounded && _canJump)
        {
            var velocity = _rb.linearVelocity;
            velocity.y = 0f;
            _rb.linearVelocity = velocity;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            _animator.SetTrigger(JumpTrigger);
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
            var extraGravity = Physics.gravity * _fallMultiplier - Physics.gravity;
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
        var inputMagnitude = _movementInput.magnitude;
        var inputX = _movementInput.x;
        var inputY = _movementInput.y;
        var speed = _rb.linearVelocity.magnitude;

        _animator.SetFloat(InputMagnitude, inputMagnitude);
        _animator.SetFloat(InputX, inputX);
        _animator.SetFloat(InputY, inputY);
        _animator.SetFloat(Speed, speed);
        _animator.SetFloat(VerticalVelocity, _rb.linearVelocity.y);

        _animator.SetBool(IsGrounded, _isGrounded);
    }
}
