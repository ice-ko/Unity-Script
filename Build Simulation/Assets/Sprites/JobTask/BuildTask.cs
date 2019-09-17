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
public class MiningTask : TaskBase
{
    public class Excavate : Task
    {
        /// <summary>
        /// 挖掘
        /// </summary>
        public Action ExcavateAction { get; set; }

    }
}