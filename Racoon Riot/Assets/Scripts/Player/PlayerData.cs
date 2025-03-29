using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _points;
    [SerializeField] private GameObject _collidingTask;
    public GameObject GetCollidingTask(){ return _collidingTask; }
    public void SetCollidingTask(GameObject collidingTask){ _collidingTask = collidingTask; }
    [SerializeField] private GameObject _collidingNode;
    public GameObject GetCollidingNode(){ return _collidingNode; }
    public void SetColldingNode(GameObject collidingNode){ _collidingNode = collidingNode; }

    [SerializeField] private GameObject _heldObject;
    public GameObject GetHeldObject(){ return _heldObject;}
    public void SetHeldObject(GameObject item){ _heldObject = item; }
    
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
