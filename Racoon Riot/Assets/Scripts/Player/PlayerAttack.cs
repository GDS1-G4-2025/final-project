using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


[RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerMovement))]
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

    void Start()
    {
        _attackCollisionHolder.GetComponent<AttackCollision>().attackDuration = _attackDuration;
    }

    public void Attack(GameObject target)
    {
        if(target.TryGetComponent<TaskData>(out TaskData taskData))
        {
            if(taskData.Active)
            {
                target.GetComponent<AttackTask>()?.PlayerAttacked(GetComponent<Player>());
                target.GetComponent<PushTask>()?.SetPlayer(GetComponent<Player>());
            }
        }
        target.GetComponent<PlayerHealth>()?.TakeDamage(_attackDamage);
        target.GetComponent<Rigidbody>()?.AddForce(gameObject.transform.forward * _knockbackForce);
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
