using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(TaskData))]
public class PushTask : MonoBehaviour
{
    private TaskData _taskData;

    public FractureOptions fractureOptions;
    private Rigidbody _rigidbody;
    [SerializeField] private Player completingPlayer;
    [SerializeField] private float _minVelocity = 7;
    [SerializeField] private bool _isDestructible;

    private void Start()
    {
        _taskData = GetComponent<TaskData>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public void SetPlayer(Player player)
    { 
        completingPlayer = player;
        _rigidbody.isKinematic = false;
    }

    public Fracture testing;
    void OnTriggerEnter(Collider other)
    {
        if(GetComponent<Rigidbody>().linearVelocity.y < -_minVelocity)
        {
            List<Player> winner = new List<Player>();
            winner.Add(completingPlayer);
            _taskData.CompleteTask(winner);
            if(_isDestructible)
            {
                gameObject.AddComponent<Destructible>();
                GetComponent<Fracture>().fractureOptions = fractureOptions;
            }
        }
    }
}
