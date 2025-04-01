using UnityEngine;

    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] public float health;
        private float _maxHealth;

        private void Start()
        {
            _maxHealth = health;
        }

        public void TakeDamage(float dmg)
        {
            health -= dmg;
            if (health <= 0)
            {
                Debug.Log("Dead");
            }
        }

        public void Reset()
        {
            health = _maxHealth;
        }
    }
