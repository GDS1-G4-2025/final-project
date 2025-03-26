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
        _attackAction = _playerInput.actions.FindAction("Fire1");
    }

    private void OnEnable()
    {
        _attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        _attackAction.performed -= OnAttackPerformed;
    }

        public void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        _isAttacking = true;
    }

    private void FixedUpdate(){
        if (_isAttacking){
            Debug.Log("Attacking");
            _attackCollisionHolder.SetActive(true);
            _isAttacking = false;
        }
    }
}
