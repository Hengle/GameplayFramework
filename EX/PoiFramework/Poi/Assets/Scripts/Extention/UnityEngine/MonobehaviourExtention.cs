using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class MonobehaviourExtention
    {
        /// <summary>
        /// 协同轮询predicate的结果，直到结果为true，执行callback；
        /// </summary>
        /// <param name="script"></param>
        /// <param name="predicate">断言方法</param>
        /// <param name="callback">回调</param>
        public static void StartCoroutine(this MonoBehaviour script, Func<bool> predicate, Action callback)
        {
            script.StartCoroutine(WaitUntil(predicate, callback));
        }

        /// <summary>
        /// 等待一定时间，然后执行回调
        /// </summary>
        /// <param name="script"></param>
        /// <param name="waittime"></param>
        /// <param name="callback">回调</param>
        public static void StartCoroutine(this MonoBehaviour script, float waittime, Action callback)
        {
            script.StartCoroutine(WaitTime(waittime, callback));
        }

        private static IEnumerator WaitTime(float waittime, Action callback)
        {
            yield return new WaitForSeconds(waittime);
            if (callback != null)
            {
                callback();
            }
        }

        private static IEnumerator WaitUntil(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);
            if (callback != null)
            {
                callback();
            }
        }
    }
}
