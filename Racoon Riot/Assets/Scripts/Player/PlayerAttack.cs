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
    [SerializeField] private float _ignoreCollisionDuration = 0.3f;
    [SerializeField] private bool _resetVelocityBeforeKnockback = true;

    private Transform _potentialTargetTransform;
    private Rigidbody _potentialTargetRigidbody;

    private bool _isAttacking;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerSnapping _playerSnapping;
    private Collider[] _myColliders;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerSnapping = GetComponent<PlayerSnapping>();
        _myColliders = GetComponentsInChildren<Collider>();

        if (_myColliders.Length == 0)
        {
            Debug.LogError("PlayerAttack could not find any colliders on the player or its children!", this);
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && !_isAttacking && (_playerSnapping == null || !_playerSnapping.IsSnapping))
        {
            if (_enableSnapping && _playerSnapping != null && _potentialTargetTransform != null)
            {
                Transform targetToSnapTo = _potentialTargetTransform;
                Rigidbody targetRigidbodyForAttack = _potentialTargetRigidbody;

                _playerSnapping.StartSnap(
                    targetToSnapTo,
                    () =>
                    {
                        // Check if the target is still valid before starting the attack
                        if (targetToSnapTo != null && targetToSnapTo.gameObject.activeInHierarchy)
                        {
                            StartCoroutine(AttackRoutine(targetRigidbodyForAttack));
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

    private IEnumerator AttackRoutine(Rigidbody targetRigidbodyToHit)
    {
        _isAttacking = true;
        _animator.SetBool(IsAttackingStr, true);
        if (_playerMovement != null) _playerMovement.enabled = false;

        _animator.SetTrigger(AttackStr);

        if (_damageApplyDelay > 0)
        {
            yield return new WaitForSeconds(_damageApplyDelay);
        }

        if (targetRigidbodyToHit != null && targetRigidbodyToHit.gameObject.activeInHierarchy)
        {
            GameObject targetObject = targetRigidbodyToHit.gameObject;

            // Apply Damage - Use the Rigidbody's GameObject
            if (targetObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(_attackDamage);
            }

            // Apply Knockback
            if (targetObject.TryGetComponent<Collider>(out Collider targetCollider) && _myColliders.Length > 0)
            {
                // Ignore Collision
                foreach (Collider attackerCol in _myColliders)
                {
                    if (attackerCol != null)
                    {
                        Physics.IgnoreCollision(attackerCol, targetCollider, true);
                    }
                }

                // Velocity Reset
                if (_resetVelocityBeforeKnockback)
                {
                    targetRigidbodyToHit.linearVelocity = Vector3.zero;
                    targetRigidbodyToHit.angularVelocity = Vector3.zero;
                }

                // Calculate direction
                Vector3 direction = (targetObject.transform.position - transform.position).normalized;
                direction.y = 0;
                if (direction == Vector3.zero) direction = transform.forward;

                targetRigidbodyToHit.AddForce(direction * _knockbackForce, ForceMode.Impulse);
                Debug.Log($"Applied Impulse Force to {targetObject.name}");

                // Re-enable Collision
                StartCoroutine(ReEnableCollisionForAll(_myColliders, targetCollider, _ignoreCollisionDuration));
            }
            else if (!targetObject.TryGetComponent<Collider>(out _))
            {
                Debug.LogWarning($"Target {targetObject.name} is missing Collider required for IgnoreCollision.");
            }
        }
        else
        {
            Debug.LogWarning("AttackRoutine: No valid target Rigidbody found/available after delay.");
        }

        // Wait for remaining duration
        float remainingTime = _attackDuration - _damageApplyDelay;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // Cleanup
        if (_playerMovement != null) _playerMovement.enabled = true;
        _isAttacking = false;
        _animator.SetBool(IsAttackingStr, false);
    }

    private IEnumerator ReEnableCollisionForAll(Collider[] attackerCols, Collider targetCol, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetCol != null)
        {
            int reEnabledCount = 0;
            foreach (Collider attackerCol in attackerCols)
            {
                if (attackerCol != null)
                {
                    if (attackerCol != null)
                    {
                        Physics.IgnoreCollision(attackerCol, targetCol, false);
                        reEnabledCount++;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Could not re-enable collision - target collider no longer exists.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform != this.transform)
        {
            // Find the Rigidbody on the entering object or its parents
            Rigidbody targetRb = other.GetComponentInParent<Rigidbody>();

            if (targetRb != null)
            {
                // Check if this Rigidbody belongs to a different player
                _potentialTargetRigidbody = targetRb;
                _potentialTargetTransform = targetRb.transform;
            }
            else
            {
                Debug.LogWarning($"Player tagged object {other.name} entered trigger but no Rigidbody found in parents.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the object leaving is associated with the currently stored Rigidbody, clear it
        if (_potentialTargetRigidbody != null && other.transform.IsChildOf(_potentialTargetRigidbody.transform))
        {
            _potentialTargetRigidbody = null;
            _potentialTargetTransform = null;
        }
        // If the root object itself leaves (if it has the collider triggering this)
        else if (_potentialTargetTransform != null && other.transform == _potentialTargetTransform)
        {
            Debug.Log($"Potential attack target RIGIDBODY cleared: {_potentialTargetRigidbody.name} (due to trigger exit of root {other.name})");
            _potentialTargetRigidbody = null;
            _potentialTargetTransform = null;
        }
    }
}
