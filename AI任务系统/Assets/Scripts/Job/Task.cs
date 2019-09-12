using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 任务类
/// </summary>
public class Task : TaskBase
{
    public class MoveToPosition : Task
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 targetPosition;
    }
    public class Victory : Task
    {
    }
    /// <summary>
    /// 清理地面任务
    /// </summary>
    public class ShellFloorCleanUp : Task
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 targetPosition;
        /// <summary>
        /// 清理动作
        /// </summary>
        public Action cleanUpAction;
    }
    /// <summary>
    /// 任务搬运武器到武器插槽
    /// </summary>
    public class TaskWeaponToWeaponSlot : Task
    {
        /// <summary>
        /// 武器位置
        /// </summary>
        public Vector3 weaponPosition;
        /// <summary>
        /// 抓住武器
        /// </summary>
        public Action<WorkerTaskAI> grabWeapon;
        /// <summary>
        /// 武器插槽位置
        /// </summary>
        public Vector3 weaponSlotPosition;
        /// <summary>
        /// 拖拽武器
        /// </summary>
        public Action dropWeapon;
    }
}
/// <summary>
/// 运输任务
/// </summary>
public class TransporterTask : TaskBase
{
    /// <summary>
    /// 运输武器到武器插槽 
    /// </summary>
    public class TakeWeaponFromSlotToPosition : TransporterTask
    {

        /// <summary>
        /// 武器位置
        /// </summary>
        public Vector3 weaponPosition;
        /// <summary>
        /// 抓住武器
        /// </summary>
        public Action<WorkerTransporterTaskAI> grabWeapon;
        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 targetPosition;
        /// <summary>
        /// 拖拽武器
        /// </summary>
        public Action dropWeapon;
    }
}