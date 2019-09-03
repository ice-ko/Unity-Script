using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem
{

    public abstract class Task
    {
        public  class MoveToPosition: Task
        {
            /// <summary>
            /// 目标位置
            /// </summary>
            public Vector3 targetPosition;
        }
        public class Victory: Task
        {
        }
    }

    private List<Task> taskList;
    public TaskSystem()
    {
        taskList = new List<Task>();
    }
    /// <summary>
    /// 请求下一个任务。
    /// </summary>
    /// <returns></returns>
    public Task RequestNextTask()
    {
        if (taskList.Count > 0)
        {
            Task task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="task">The task.</param>
    public void AddTask(Task task)
    {
        taskList.Add(task);
    }
}
