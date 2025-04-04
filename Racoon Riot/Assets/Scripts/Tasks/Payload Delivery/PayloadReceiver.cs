using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TaskData))]
public class PayloadReceiver : MonoBehaviour
{
    private TaskData _taskData;
    [SerializeField] private List<Payload> _payloadsRemaining;
    private void Start()
    {
        _taskData = GetComponent<TaskData>();
    }

    public void AddPayload(Payload payload){ _payloadsRemaining.Add(payload); }
    public bool AttemptTask(List<Player> players)
    {
        foreach(Player p in players)
        {
            if(p.hold.heldObject != null)
            {
                if(_payloadsRemaining.Contains(p.hold.heldObject.GetComponent<Payload>()))
                {
                    _payloadsRemaining.Remove(p.hold.heldObject.GetComponent<Payload>());
                    p.hold.heldObject.AttachTo(this.gameObject);
                    p.hold.heldObject.GetComponent<Collider>().enabled = false;;
                    p.hold.heldObject = null;
                }
                if(_payloadsRemaining.Count == 0)
                { 
                    List<Player> winner = new List<Player>();
                    winner.Add(p);
                    _taskData.CompleteTask(winner); 
                    return true;
                }
            }
        }
        return false;
    }
}
