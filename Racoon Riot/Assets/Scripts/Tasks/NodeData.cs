using UnityEngine;

public class NodeData : MonoBehaviour
{
    [SerializeField] private GameObject _parentTask;
    public GameObject GetParentTask(){ return _parentTask; }

    void Start()
    {
        _parentTask.GetComponent<TaskData>().AddNode(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
