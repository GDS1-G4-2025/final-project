using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(TaskData))]
public class PushTask : MonoBehaviour
{
    private TaskData _taskData;
    [SerializeField] private float _knockbackForce;

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

    public void HitByPlayer(Player player)
    { 
        Debug.Log("pushing");
        completingPlayer = player;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(player.transform.forward * _knockbackForce);
    }

    void OnTriggerEnter(Collider other)
    {
        if(GetComponent<Rigidbody>().linearVelocity.y < -_minVelocity && _taskData.Active)
        {
            List<Player> winner = new List<Player>();
            winner.Add(completingPlayer);
            _taskData.CompleteTask(winner);
            if(_isDestructible)
            {
                Fracture fracture = GetComponent<Fracture>();
                fracture.CauseFracture();
            }
        }
    }
}
