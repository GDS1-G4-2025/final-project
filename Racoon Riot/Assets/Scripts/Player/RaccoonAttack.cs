using UnityEngine;
using UnityEngine.InputSystem;

public class RaccoonAttack : MonoBehaviour
{
    [SerializeField] private GameObject _attackCollisionHolder;
    [SerializeField] private float _attackDuration, _attackDamage;
    public float GetAttackDuration(){ return _attackDuration; }
    public float GetAttackDamage(){ return _attackDamage; }
    
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private bool _isAttacking = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions.FindAction("Attack");
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
        _isAttacking = true;
    }

    void Update() 
    {
        if (_isAttacking){
            Debug.Log("Attacking");
            _attackCollisionHolder.SetActive(true);
            _isAttacking = false;
        }
    }
}
