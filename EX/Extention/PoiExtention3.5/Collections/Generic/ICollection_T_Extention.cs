using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// 集合容器扩展（主要用于List）
    /// </summary>
    public static class ICollection_T_Extention
    {
        /// <summary>
        /// 通过线程池异步移除一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ic"></param>
        /// <param name="item"></param>
        public static void RemoveInForeach<T>(this ICollection<T> ic,T item)
        {
            Threading.ThreadPool.QueueUserWorkItem((obj)=> 
            {
                lock (ic)
                {
                    ic.Remove(item);
                }
            });
        }

        /// <summary>
        /// 通过线程池异步添加一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ic"></param>
        /// <param name="item"></param>
        public static void AddInForeach<T>(this ICollection<T> ic, T item)
        {
            Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                lock (ic)
                {
                    ic.Add(item);
                }
            });
        }
    }
}
