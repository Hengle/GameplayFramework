using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// MonoBehaviour 扩展
    /// </summary>
    public static class MonoBehaviourExtention
    {
        /// <summary>
        /// 取得一个组件，如果没有就添加这个组件
        /// </summary>
        /// <typeparam name="T">目标组件</typeparam>
        /// <param name="monoBehaviour"></param>
        /// <returns>目标组件</returns>
        public static T GetComponentIfNullAdd<T>(this Behaviour monoBehaviour)
            where T:Component
        {
            var com = monoBehaviour.GetComponent<T>();
            if (com == null)
            {
                com = monoBehaviour.gameObject.AddComponent<T>();
            }

            return com;
        }

        /// <summary>
        /// MonoBehaviour自身和继承的属性名字列表
        /// </summary>
        public static List<string> PropertiesNames = new List<string>()
        {
            "gameObject","tag","name","hideFlags","transform",

            "useGUILayout","enabled","isActiveAndEnabled","runInEditMode",
            

            "animation","audio","camera","collider","collider2D","constantForce","guiElement",
            "guiText","guiTexture","hingeJoint","light","networkView","particleEmitter",
            "particleSystem","renderer","rigidbody","rigidbody2D",
        };

        public static void Wait(this MonoBehaviour monoscript,AsyncOperation asyncOperation, Action Callback)
        {
            monoscript.StartCoroutine(Func(asyncOperation, Callback));
        }

        static IEnumerator Func(AsyncOperation asyncOperation, Action callback, float waitTime = -1)
        {
            if (asyncOperation != null)
            {
                while (!asyncOperation.isDone)
                {
                    if (waitTime > 0)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                    else
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            if (callback != null)
            {
                callback.Invoke();
            }

        }

        public static void Wait(this MonoBehaviour monoscript, float waitTime, Action Callback)
        {
            monoscript.StartCoroutine(Func(waitTime, Callback));
        }

        static IEnumerator Func(float waitTime, Action callback)
        {
            yield return new WaitForSeconds(waitTime);

            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }
}
