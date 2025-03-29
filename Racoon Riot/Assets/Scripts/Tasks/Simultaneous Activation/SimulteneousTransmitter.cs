using UnityEngine;

public class SimulteneousTransmitter : MonoBehaviour
{
    [SerializeField] private bool _nodeActive;
    [SerializeField] private NodeData _nodeData;

    void Start()
    {
        _nodeData = this.gameObject.GetComponent<NodeData>();
    }

    void FixedUpdate()
    {
        if(_nodeData.GetActivateNode() && !_nodeActive){
            _nodeData.GetParentTask().GetComponent<SimultaneousReceiver>().AdjustNodeActive(+1);
            _nodeActive = true;
        }
        else if(!_nodeData.GetActivateNode() && _nodeActive){
            _nodeData.GetParentTask().GetComponent<SimultaneousReceiver>().AdjustNodeActive(-1);
            _nodeActive = false;
        }
    }
}
