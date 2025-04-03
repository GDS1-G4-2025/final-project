using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


[RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerSnapping))]
public class PlayerAttack : MonoBehaviour
{
    private static readonly int IsAttackingStr = Animator.StringToHash("isAttacking");
    private static readonly int AttackStr = Animator.StringToHash("attack");

    //[SerializeField] private GameObject _attackCollisionHolder;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private bool _enableSnapping = true;

    private Transform _potentialAttackTarget;

    private bool _isAttacking;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerSnapping _playerSnapping;

    public void Attack(GameObject target)
    {
        target.GetComponent<PlayerHealth>().TakeDamage(_attackDamage);

        //Knockback
        if (target.TryGetComponent<Rigidbody>(out Rigidbody targetRigidbody))
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0;
            if (direction == Vector3.zero) direction = transform.forward; // Fallback if overlapping

            targetRigidbody.AddForce(direction * _knockbackForce);
        }
    }

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
                            StartCoroutine(AttackRoutine());
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
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        _animator.SetBool(IsAttackingStr, true);

        //Disabling movement
        _playerMovement.enabled = false;

        Debug.Log("Attacking");
        _animator.SetTrigger(AttackStr);
        //_attackCollisionHolder.SetActive(true);

        yield return new WaitForSeconds(_attackDuration);

        //Enabling movement
        _playerMovement.enabled = true;

        _isAttacking = false;
        _animator.SetBool(IsAttackingStr, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _potentialAttackTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _potentialAttackTarget = null;
        }
    }
}
