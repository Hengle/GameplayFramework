using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class GameObjectExtention
    {
        /// <summary>
        /// 取得一个组件，如果没有就添加这个组件
        /// </summary>
        /// <typeparam name="T">目标组件</typeparam>
        /// <param name="go"></param>
        /// <returns>目标组件</returns>
        public static T GetComponentIfNullAdd<T>(this GameObject go)
            where T : Component
        {
            var com = go.GetComponent<T>();
            if (com == null)
            {
                com = go.AddComponent<T>();
            }

            return com;
        }

        /// <summary>
        /// 更改包括子的层级
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        public static void SetLayerOnAll(this GameObject obj, int layer)
        {
            if (null == obj)
                return;

            foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
            {
                ///据说GetComponentsInChildren更快
                trans.gameObject.layer = layer;
            }
        }
    }
}
