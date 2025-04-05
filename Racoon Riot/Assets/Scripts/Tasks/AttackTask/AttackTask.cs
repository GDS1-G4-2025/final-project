using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class AttackTask : MonoBehaviour
{
    private TaskData _taskData;

    [SerializeField] private bool _isDestructible;
    [HideInInspector] public int currentHits;
    [SerializeField] private int hitsRequired;
    [SerializeField] private int _minPlayersRequired, _maxPlayersRequired;
    public List<Player> players;

    void Start()
    {
        _taskData = GetComponent<TaskData>();
    }

    public void PlayerAttacked(Player player)
    {
        if(players.Count > _maxPlayersRequired)
        {
            players.RemoveAt(0);
        }
        players.Add(player);

        currentHits += 1;
        if(currentHits >= hitsRequired && players.Count > _minPlayersRequired)
        {
            _taskData.CompleteTask(players);
            if(_isDestructible)
            {
                GetComponent<Fracture>().CauseFracture();
            }
        }
    }
}
