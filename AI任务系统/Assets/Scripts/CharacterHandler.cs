using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public static CharacterHandler Instance;
    /// <summary>
    /// 金矿位置
    /// </summary>
    public Transform goldNode;
    /// <summary>
    /// 储存点
    /// </summary>
    public Transform warehouse;

    public GameObject character;
    TaskSystem taskSystem;
    private void Awake()
    {
        Instance = this;
        taskSystem = new TaskSystem();

        Worker worker = Worker.Create(character, new Vector3(0, 0));
        WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
        workerTaskAI.Setup(worker, taskSystem);

        //worker = Worker.Create(character, new Vector3(3, 3));
        //workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
        //workerTaskAI.Setup(worker, taskSystem);
        //
        //FunctionTimer.Create(() =>
        //{
        //    CMDebug.TextPopupMouse("添加任务");
        //    TaskSystem.Task task = new TaskSystem.Task
        //    {
        //        targetPosition = new Vector3(10, 10)
        //    };
        //    taskSystem.AddTask(task);
        //}, 1f);

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TaskSystem.Task task = new TaskSystem.Task.MoveToPosition
            {
                targetPosition = UtilityClass.GetMouseWorldPos()
            };
            taskSystem.AddTask(task);
        }
        if (Input.GetMouseButtonDown(1))
        {
            TaskSystem.Task task = new TaskSystem.Task.Victory
            {
            };
            taskSystem.AddTask(task);
        }
    }
}

