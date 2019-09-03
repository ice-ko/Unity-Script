/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

namespace CM_TaskSystem {
    
    public class QueuedTask<TTask> where TTask : TaskBase {

        private Func<TTask> tryGetTaskFunc;

        public QueuedTask(Func<TTask> tryGetTaskFunc) {
            this.tryGetTaskFunc = tryGetTaskFunc;
        }

        public TTask TryDequeueTask() {
            return tryGetTaskFunc();
        }
    }

    // Base Task class
    public abstract class TaskBase {

    }

    public class CM_TaskSystem<TTask> where TTask : TaskBase {

        private List<TTask> taskList; // List of tasks ready to be executed
        private List<QueuedTask<TTask>> queuedTaskList; // Any queued task must be validated before being dequeued

        public CM_TaskSystem() {
            taskList = new List<TTask>();
            queuedTaskList = new List<QueuedTask<TTask>>();
            FunctionPeriodic.Create(DequeueTasks, .2f);
        }

        public TTask RequestNextTask() {
            // Worker requesting a task
            if (taskList.Count > 0) {
                // Give worker the first task and remove it from the list
                TTask task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            } else {
                // No tasks are available
                return null;
            }
        }

        public void AddTask(TTask task) {
            taskList.Add(task);
        }

        public void EnqueueTask(QueuedTask<TTask> queuedTask) {
            queuedTaskList.Add(queuedTask);
        }

        public void EnqueueTask(Func<TTask> tryGetTaskFunc) {
            QueuedTask<TTask> queuedTask = new QueuedTask<TTask>(tryGetTaskFunc);
            queuedTaskList.Add(queuedTask);
        }

        private void DequeueTasks() {
            for (int i = 0; i < queuedTaskList.Count; i++) {
                QueuedTask<TTask> queuedTask = queuedTaskList[i];
                TTask task = queuedTask.TryDequeueTask();
                if (task != null) {
                    // Task dequeued! Add to normal list
                    AddTask(task);
                    queuedTaskList.RemoveAt(i);
                    i--;
                    CMDebug.TextPopupMouse("Task Dequeued");
                } else {
                    // Returned task is null, keep it queued
                }
            }
        }

    }

}