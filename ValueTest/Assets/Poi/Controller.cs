using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public class Controller:MonoBehaviour
    {
        public bool IsFollowPawn { get; set; }

        protected Pawn pawn;
        protected Pawn oldPawn;

        public Pawn Pawn => pawn;

        public void Possess(Pawn pawn)
        {
            oldPawn = this.pawn;
            this.pawn = pawn;
        }

        public void UnPossess()
        {

        }


        public static T CreateController<T>()
            where T : Controller
        {
            GameObject go = new GameObject("Controller");
            T tempCtrl = go.AddComponent<T>();

            Register(tempCtrl);

            return tempCtrl;
        }

        /// <summary>
        /// 控制器集合
        /// </summary>
        public static List<Controller> Controllers => controllers;
        static readonly List<Controller> controllers = new List<Controller>();


        private static void Register<T>(T tempCtrl) where T : Controller
        {
            lock (Controllers)
            {
                Controllers.Add(tempCtrl);
            }
        }

        private static void UnRegister<T>(T tempCtrl) where T : Controller
        {
            lock (Controllers)
            {
                Controllers.Remove(tempCtrl);
            }
        }

        // 当 MonoBehaviour 将被销毁时调用此函数
        protected virtual void OnDestroy()
        {
            Controllers.Remove(this);
        }

    }
}
