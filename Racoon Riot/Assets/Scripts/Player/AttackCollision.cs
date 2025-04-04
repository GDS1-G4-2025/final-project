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
        if(other.TryGetComponent<AttackTask>(out AttackTask task))
        {
            if(other.GetComponent<TaskData>().Active)
            {
                task.PlayerAttacked(_attackHandler.GetComponent<Player>());
            }
        }
        if(other.transform.CompareTag("Player"))
        {
            _attackHandler.Attack(other.gameObject);
        }
    }
}
