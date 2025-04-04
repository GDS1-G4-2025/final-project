using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private PlayerAttack _attackHandler;
    public float attackDuration;
    private void OnEnable()
    {
        Invoke(nameof(DisableSelf), attackDuration);
    }

    private void DisableSelf(){
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _attackHandler.Attack(other.gameObject);
    }
}
