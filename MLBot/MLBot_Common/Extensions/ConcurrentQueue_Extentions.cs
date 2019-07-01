using MLBot.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MLBot.Extentions
{
    /// <summary>
    /// 多线程队列
    /// </summary>
    [Author("Linyee", "2019-06-05")]
    public static class ConcurrentQueue_Extentions
    {
        /// <summary>
        /// 清空多线程队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cq"></param>
        public static void Clear<T>(this ConcurrentQueue<T> cq){
            if (cq != null)
            {
                T res =default(T);
                while (cq.Count > 0)
                {
                    cq.TryDequeue(out res);
                }
            }
        }
    }
}
