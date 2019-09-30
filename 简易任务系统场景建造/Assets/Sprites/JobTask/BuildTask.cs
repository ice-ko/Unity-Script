using System;
using UnityEngine;

public class BuildTask : TaskBase
{
    /// <summary>
    /// 检查资源和建造
    /// </summary>
    public class CheckResourceAndBuild : Task
    {
        /// <summary>
        /// 检测
        /// </summary>
        public Action CheckAction { get; set; }

    }
}
/// <summary>
/// 采矿任务
/// </summary>
public class MiningTask : TaskBase
{
    /// <summary>
    /// 挖掘
    /// </summary>
    public class Excavate : Task
    {
        /// <summary>
        /// 挖掘
        /// </summary>
        public Action<Excavate> ExcavateAction { get; set; }
        /// <summary>
        /// 矿石类型
        /// </summary>
        public object Type { get; set; }
        /// <summary>
        /// 工作量（矿石采集时间）
        /// </summary>
        public float Workload { get; set; }
    }
}