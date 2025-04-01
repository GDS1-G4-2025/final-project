// using UnityEngine;

// [RequireComponent(typeof(NodeData))]
// public class SimultaneousTransmitter : MonoBehaviour
// {
//     [SerializeField] private bool _nodeActive;
//     [SerializeField] private NodeData _nodeData;
//     private SimultaneousReceiver _simultaneousReceiver;

//     private void Start()
//     {
//         _nodeData = gameObject.GetComponent<NodeData>();
//         _simultaneousReceiver = _nodeData.parentTask.GetComponent<SimultaneousReceiver>();
//     }

//     public void OnNodeStateChanged(bool isActive)
//     {
//         if (isActive && !_nodeActive)
//         {
//             _simultaneousReceiver?.AdjustActiveNodes(+1);
//             _nodeActive = true;
//         }
//         else if (!isActive && _nodeActive)
//         {
//             _simultaneousReceiver?.AdjustActiveNodes(-1);
//             _nodeActive = false;
//         }
//     }
// }
