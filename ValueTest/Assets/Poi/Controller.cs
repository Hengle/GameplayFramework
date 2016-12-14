using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public class Controller:MonoBehaviour
    {
        public static T CreateController<T>()
            where T : Controller
        {
            GameObject go = new GameObject("Controller");
            T tempCtrl = go.AddComponent<T>();

            Register(tempCtrl);

            return tempCtrl;
        }

        private static void Register<T>(T tempCtrl) where T : Controller
        {
            GM.Controllers.Add(tempCtrl);
        }

        private static void UnRegister<T>(T tempCtrl) where T : Controller
        {
            GM.Controllers.Add(tempCtrl);
        }

        // 当 MonoBehaviour 将被销毁时调用此函数
        protected virtual void OnDestroy()
        {
            GM.Controllers.Remove(this);
        }

    }
}
