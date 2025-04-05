using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float health;
    private float _maxHealth;
    private PlayerPickupThrow _pickupSystem;

    private void Start()
    {
        _maxHealth = health;
        _pickupSystem = GetComponent<PlayerPickupThrow>();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (_pickupSystem != null && _pickupSystem.IsHoldingObject())
        {
            _pickupSystem.DropHeldObject();
        }
        if (health <= 0)
        {
            Debug.Log("Dead");
        }
    }

    public void RestoreHealth(float amount)
    {
        health = Mathf.Min(health + amount, _maxHealth);
        Debug.Log($"Healed {amount}. Current health: {health}");
    }

    public void Reset()
    {
        health = _maxHealth;
    }
}
