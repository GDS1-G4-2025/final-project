using UnityEngine;

[
    RequireComponent(typeof(PlayerAttack)),
    RequireComponent(typeof(PlayerHealth)),
    RequireComponent(typeof(PlayerPickupThrow)),
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
    public PlayerPickupThrow hold;
    [HideInInspector]
    public PlayerInteraction interaction;
    [HideInInspector]
    public PlayerMovement movement;
    [HideInInspector]
    public PlayerScore score;

    // Data
    public TaskData collidingTask;
    public NodeData collidingNode;

    private void Awake()
    {
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        hold = GetComponent<PlayerPickupThrow>();
        interaction = GetComponent<PlayerInteraction>();
        movement = GetComponent<PlayerMovement>();
        score = GetComponent<PlayerScore>();
    }
}
