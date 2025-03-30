using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RaccoonAttack : MonoBehaviour
{
    [SerializeField] private GameObject _attackCollisionHolder;
    [SerializeField] private float _attackDuration, _attackDamage, _knockbackForce;

    public float GetAttackDuration(){ return _attackDuration; }
    public float GetAttackDamage(){ return _attackDamage; }
    public float GetKnockbackForce(){ return _knockbackForce; }
    
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private bool _isAttacking = false;
    private Animator _animator;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions.FindAction("Attack");
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _attackAction.performed += OnAttack;
    }

    private void OnDisable()
    {
        _attackAction.performed -= OnAttack;
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!_isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        _animator.SetBool("isAttacking", true);

        //Disabling movement
        _playerInput.actions["Move"].Disable();
        _playerInput.actions["Jump"].Disable();

        Debug.Log("Attacking");
        _animator.SetTrigger("attack");
        _attackCollisionHolder.SetActive(true);

        yield return new WaitForSeconds(_attackDuration);

        //Enabling movement
        _playerInput.actions["Move"].Enable();
        _playerInput.actions["Jump"].Enable();

        _isAttacking = false;
        _animator.SetBool("isAttacking", false);
    }
}
