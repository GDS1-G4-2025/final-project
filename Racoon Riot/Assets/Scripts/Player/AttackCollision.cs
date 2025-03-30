using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private RaccoonAttack _attackHandler;
    [SerializeField] private float _attackDuration, _attackDamage, _knockbackForce;
    void OnEnable()
    {
        _attackDuration = _attackHandler.GetAttackDuration();
        _attackDamage = _attackHandler.GetAttackDamage();
        _knockbackForce = _attackHandler.GetKnockbackForce();
        Invoke("DisableSelf", _attackDuration);
    }

    void DisableSelf(){
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerData>().TakeDamage(_attackDamage);
            other.gameObject.GetComponent<Rigidbody>().AddForce(_attackHandler.gameObject.transform.forward*_knockbackForce);
            DisableSelf();
        }
    }
}
