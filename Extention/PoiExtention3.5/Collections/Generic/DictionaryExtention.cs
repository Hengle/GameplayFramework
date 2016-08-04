using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class DictionaryExtention
    {
        /// <summary>
        /// 通过线程池异步移除一个元素
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="idic"></param>
        /// <param name="key"></param>
        public static void RemoveForeach<K,V>(this IDictionary<K,V> idic, K key)
        {
            Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                lock (idic)
                {
                    idic.Remove(key);
                }
            });
        }

        /// <summary>
        /// 通过线程池异步添加一个元素
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="idic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="overlay"></param>
        public static void AddForeach<K, V>(this IDictionary<K, V> idic, K key,V value,bool overlay = true)
        {
            Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                lock (idic)
                {
                    if (overlay)
                    {
                        idic[key] = value;
                    }
                    else
                    {
                        if (idic.ContainsKey(key))
                        {
                            return;
                        }
                        idic[key] = value;
                    } 
                }
            });
        }
    }
}
