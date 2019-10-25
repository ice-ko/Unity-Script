using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Timers
{
    /// <summary>
    /// 当定时器达到时间后触发
    /// </summary>
    public delegate void TimeDelegate();
    /// <summary>
    /// 定时器任务数据信息
    /// </summary>
    public class TimerModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 任务执行时间
        /// </summary>
        public long Time { get; set; }

        public TimeDelegate timeDelegate { get; set; }

        public TimerModel(int id, long time, TimeDelegate timeDelegate)
        {
            this.Id = id;
            this.Time = time;
            this.timeDelegate = timeDelegate;
        }
        /// <summary>
        /// 触发任务
        /// </summary>
        public void Run()
        {
            timeDelegate();
        }
    }
}
