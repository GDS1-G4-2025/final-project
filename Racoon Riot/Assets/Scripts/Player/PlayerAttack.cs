using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


[RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerSnapping))]
public class PlayerAttack : MonoBehaviour
{
    private static readonly int IsAttackingStr = Animator.StringToHash("isAttacking");
    private static readonly int AttackStr = Animator.StringToHash("attack");

    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private bool _enableSnapping = true;
    [SerializeField] private float _damageApplyDelay = 0.2f;

    private Transform _potentialAttackTarget;

    private bool _isAttacking;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerSnapping _playerSnapping;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSnapping = GetComponent<PlayerSnapping>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && !_isAttacking && (_playerSnapping == null || !_playerSnapping.IsSnapping))
        {
            if (_enableSnapping && _playerSnapping != null && _potentialAttackTarget != null)
            {
                Transform targetToSnapTo = _potentialAttackTarget;

                _playerSnapping.StartSnap(
                    targetToSnapTo,
                    () =>
                    {
                        // Check if the target is still valid before starting the attack
                        if (targetToSnapTo != null && targetToSnapTo.gameObject.activeInHierarchy)
                        {
                            Debug.Log("Snap complete. Starting AttackRoutine.");
                            StartCoroutine(AttackRoutine(targetToSnapTo));
                        }
                        else
                        {
                            Debug.Log("Snap target became invalid before AttackRoutine could start.");
                        }
                    }
                );
            }
            else
            {
                StartCoroutine(AttackRoutine(null));
            }
        }
    }

    private IEnumerator AttackRoutine(Transform snappedTarget)
    {
        _isAttacking = true;
        _animator.SetBool(IsAttackingStr, true);

        //Disabling movement
        if (_playerMovement != null) _playerMovement.enabled = false;

        Debug.Log("Attacking");
        _animator.SetTrigger(AttackStr);

        if (_damageApplyDelay > 0)
        {
            yield return new WaitForSeconds(_damageApplyDelay);
        }

        Transform targetToHit = snappedTarget ?? _potentialAttackTarget;

        if (targetToHit != null && targetToHit.gameObject.activeInHierarchy)
        {
            GameObject targetObject = targetToHit.gameObject;

            // Apply Damage
            if (targetObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(_attackDamage);
            }

            // Apply Knockback
            if (targetObject.TryGetComponent<Rigidbody>(out Rigidbody targetRigidbody))
            {
                Vector3 direction = (targetObject.transform.position - transform.position).normalized;
                direction.y = 0; // Keep knockback horizontal
                if (direction == Vector3.zero) direction = transform.forward;

                targetRigidbody.AddForce(direction * _knockbackForce);
            }
        }

        float remainingTime = _attackDuration - _damageApplyDelay;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        //Enabling movement
        if (_playerMovement != null) _playerMovement.enabled = true;

        _isAttacking = false;
        _animator.SetBool(IsAttackingStr, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && other.transform != this.transform)
        {
            _potentialAttackTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") && other.transform == _potentialAttackTarget)
        {
            _potentialAttackTarget = null;
        }
    }
}
