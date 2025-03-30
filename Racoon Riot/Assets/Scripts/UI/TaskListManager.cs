using TMPro;
using UnityEngine;

public class TaskListManager : MonoBehaviour
{
    private TaskManager _taskManager;
    [SerializeField] private TMP_Text[] listItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _taskManager = Object.FindAnyObjectByType<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < listItems.Length; i++){
            if(i < _taskManager.GetCurrentTasks().Count){
                listItems[i].text = _taskManager.GetCurrentTasks()[i].GetComponent<TaskData>().GetTaskName();
            }
            else{
                listItems[i].text = "";
            }
        }
    }
}
