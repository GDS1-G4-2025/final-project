using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private RaccoonAttack _attackHandler;
    [SerializeField] private float _attackDuration, _attackDamage;
    void OnEnable()
    {
        _attackDuration = _attackHandler.GetAttackDuration();
        _attackDamage = _attackHandler.GetAttackDamage();
        Invoke("DisableSelf", _attackDuration);
    }

    void DisableSelf(){
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerData>().TakeDamage(_attackDamage);
            DisableSelf();
        }
    }
}
