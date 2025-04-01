using UnityEngine;

[
    RequireComponent(typeof(PlayerAttack)),
    RequireComponent(typeof(PlayerHealth)),
    RequireComponent(typeof(PlayerHoldObject)),
    RequireComponent(typeof(PlayerInteraction)),
    RequireComponent(typeof(PlayerMovement)),
    RequireComponent(typeof(PlayerScore))
]
public class Player : MonoBehaviour
{
    // Modules
    [HideInInspector]
    public PlayerAttack attack;
    [HideInInspector]
    public PlayerHealth health;
    [HideInInspector]
    public PlayerHoldObject hold;
    [HideInInspector]
    public PlayerInteraction interaction;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerScore score;

    // Data
    public TaskData collidingTask;

    private void Awake()
    {
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        hold = GetComponent<PlayerHoldObject>();
        interaction = GetComponent<PlayerInteraction>();
        movement = GetComponent<PlayerMovement>();
        score = GetComponent<PlayerScore>();
    }
}
