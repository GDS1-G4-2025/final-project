using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float _health;
    private float _maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
