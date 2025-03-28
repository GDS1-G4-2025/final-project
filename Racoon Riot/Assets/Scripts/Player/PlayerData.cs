using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _points;
    [SerializeField] private GameObject _collidingTask;
    public GameObject GetCollidingTask(){ return _collidingTask; }
    public void SetCollidingTask(GameObject collidingTask){ _collidingTask = collidingTask; }
    
    private float _maxHealth;

    void Start()
    {
        _maxHealth = _health;
    }

    public void TakeDamage(float dmg){
        _health -= dmg;
        if(_health <= 0){
            Debug.Log("Dead");
        }
    }

    public void AddPoints(int points){
        _points += points;
    }
}
