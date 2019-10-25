using Server.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Server.Timers
{
    /// <summary>
    /// 定时任务管理器
    /// </summary>
    public class TimerManager
    {
        private static TimerManager instance;
        public static TimerManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new TimerManager();
                    }
                    return instance;
                }
            }
        }
        /// <summary>
        /// 实现定时的主要功能
        /// </summary>
        Timer timer;
        /// <summary>
        /// 任务字典
        /// </summary>
        ConcurrentDictionary<int, TimerModel> idModelDict = new ConcurrentDictionary<int, TimerModel>();
        /// <summary>
        /// 要移除的任务id列表
        /// </summary>
        List<int> removeList = new List<int>();

        ConcurrentInt id = new ConcurrentInt(-1);
        public TimerManager()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
        }
        /// <summary>
        /// 到达时间间隔的时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (removeList)
            {
                TimerModel tmpModel;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out tmpModel);
                }
                removeList.Clear();
            }

            foreach (var model in idModelDict.Values)
            {
                if (model.Time <= DateTime.Now.Ticks)
                {
                    model.Run();
                }
            }
        }
        /// <summary>
        /// 添加定时任务 指定触发时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeDelegate"></param>
        private void AddTimerEvent(DateTime dateTime, TimeDelegate timeDelegate)
        {
            long delayTime = dateTime.Ticks - DateTime.Now.Ticks;
        }
        /// <summary>
        /// 添加定时任务 指定延迟时间
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="timeDelegate"></param>
        public void AddTimerEvent(long delayTime, TimeDelegate timeDelegate)
        {
            TimerModel model = new TimerModel(id.Add_Get(), DateTime.Now.Ticks + delayTime, timeDelegate);
            idModelDict.TryAdd(model.Id, model);
        }
    }
}
