using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private static readonly int IsAttackingStr = Animator.StringToHash("isAttacking");
    private static readonly int AttackStr = Animator.StringToHash("attack");

    [SerializeField] private GameObject _attackCollisionHolder;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _knockbackForce;

    private bool _isAttacking;
    private Animator _animator;
    private PlayerMovement _playerMovement;

    public void Attack(GameObject target)
    {
        target.GetComponent<PlayerHealth>().TakeDamage(_attackDamage);
        target.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * _knockbackForce);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
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
        _animator.SetBool(IsAttackingStr, true);

        //Disabling movement
        _playerMovement.enabled = false;

        Debug.Log("Attacking");
        _animator.SetTrigger(AttackStr);
        _attackCollisionHolder.SetActive(true);

        yield return new WaitForSeconds(_attackDuration);

        //Enabling movement
        _playerMovement.enabled = true;

        _isAttacking = false;
        _animator.SetBool(IsAttackingStr, false);
    }
}
