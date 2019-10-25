using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server.Pool
{
    public delegate void ExecutDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExecute
    {
        private static object o = 1;
        private static SingleExecute instance;
        public static SingleExecute Instance
        {
            get
            {
                lock (o)
                {
                    if (instance == null)
                    {
                        instance = new SingleExecute();
                    }
                    return instance;
                }
            }
        }
        /// <summary>
        /// 互斥锁
        /// </summary>
        public Mutex mutex;
        public SingleExecute()
        {
            mutex = new Mutex();
        }
        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="executDelegate"></param>
        public void Execute(ExecutDelegate executDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();

                executDelegate();

                mutex.ReleaseMutex();
            }
        }
    }
}
