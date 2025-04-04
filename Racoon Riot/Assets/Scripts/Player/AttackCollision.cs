using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private PlayerAttack _attackHandler;
    [SerializeField] private float _attackDuration;
    private void OnEnable()
    {
        Invoke(nameof(DisableSelf), _attackDuration);
    }

    private void DisableSelf(){
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            _attackHandler.Attack(other.gameObject);
            DisableSelf();
        }
        if(other.TryGetComponent<AttackTask>(out AttackTask task))
        {
            Debug.Log("hitTask");
            task.PlayerAttacked(_attackHandler.GetComponent<Player>());
        }
    }
}
