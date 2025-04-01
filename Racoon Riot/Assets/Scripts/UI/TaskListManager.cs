using TMPro;
using UnityEngine;

public class TaskListManager : MonoBehaviour
{
    private TaskManager _taskManager;
    [SerializeField] private TMP_Text[] _listItems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _taskManager = FindAnyObjectByType<TaskManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        for (var i = 0; i < _listItems.Length; i++)
        {
            _listItems[i].text = i < _taskManager.CurrentTasks.Count ? _taskManager.CurrentTasks[i].GetComponent<TaskData>().taskName : "";
        }
    }
}
