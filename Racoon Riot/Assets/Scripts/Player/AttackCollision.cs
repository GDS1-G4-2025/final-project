using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private float _attackDuration, _attackDamage;
    void OnEnable()
    {
        Invoke("DisableSelf", _attackDuration);
    }

    void DisableSelf(){
        this.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerData>().TakeDamage(_attackDamage);
            DisableSelf();
        }
    }
}
