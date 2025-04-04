using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTaskAttack : MonoBehaviour
{
    [SerializeField] private TaskData _taskTarget;

    public void OnTaskAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (_taskTarget != null)
            {
                TaskAttackTriggers(_taskTarget);
            }
            else if (ctx.phase == InputActionPhase.Started && _taskTarget != null)
            {
                Debug.Log("Destruction input ignored: Player is currently snapping.");
            }
        }
    }

    public void TaskAttackTriggers(TaskData taskData)
    {
        if(!taskData.Active){ return; }
        taskData.GetComponent<AttackTask>()?.PlayerAttacked(GetComponent<Player>());
        taskData.GetComponent<PushTask>()?.HitByPlayer(GetComponent<Player>());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out TaskData task))
        {
            _taskTarget = task;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_taskTarget != null && _taskTarget.gameObject == other.gameObject)
        {
            _taskTarget = null;
        }
    }
}
