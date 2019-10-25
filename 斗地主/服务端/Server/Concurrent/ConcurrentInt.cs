using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Concurrent
{
    /// <summary>
    /// 线程安全的int类型
    /// </summary>
    public class ConcurrentInt
    {
        private int value;
        public ConcurrentInt(int value)
        {
            this.value = value;
        }
        /// <summary>
        /// 添加并获取值
        /// </summary>
        /// <returns></returns>
        public int Add_Get()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }
        /// <summary>
        /// 减少并获取
        /// </summary>
        /// <returns></returns>
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return value;
        }
    }
}
