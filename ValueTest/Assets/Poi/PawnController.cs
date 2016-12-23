using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class PawnController:MonoBehaviour
    {
        public bool IsFollowPawn { get; set; }

        /// <summary>
        ///控制模式
        /// </summary>
        public ControlMode Mode { get; set; }


        protected Pawn pawn;
        protected Pawn oldPawn;

        public Pawn Pawn => pawn;
        public Pawn OldPawn => oldPawn;

        /// <summary>
        /// 控制角色
        /// </summary>
        /// <param name="pawn"></param>
        public virtual bool Possess(Pawn pawn)
        {
            ///贪婪控制器处理
            if (pawn?.Controller?.Mode == ControlMode.Greedy)
            {
                return false;
            }

            ///前控制器释放
            pawn?.Controller?.UnPossess();

            oldPawn = this.pawn;
            this.pawn = pawn;

            pawn.Controller = this;

            if (IsFollowPawn)
            {
                ///控制器跟随
                transform.SetParent(pawn.transform);

                transform.ResetLocal();
                
            }

            return true;
        }

        /// <summary>
        /// 释放角色
        /// </summary>
        /// <returns></returns>
        public Pawn UnPossess()
        {
            if (transform.parent = Pawn.transform)
            {
                transform.SetParent(null);
            }


            pawn.Controller = null;
            oldPawn = Pawn;
            pawn = null;
            return OldPawn;
        }


        public static T CreateController<T>()
            where T : PawnController
        {
            GameObject go = new GameObject("Controller");
            T tempCtrl = go.AddComponent<T>();

            Register(tempCtrl);

            return tempCtrl;
        }

        /// <summary>
        /// 控制器集合
        /// </summary>
        public static List<PawnController> Controllers => controllers;
        static readonly List<PawnController> controllers = new List<PawnController>();


        private static void Register<T>(T tempCtrl) where T : PawnController
        {
            lock (Controllers)
            {
                Controllers.Add(tempCtrl);
            }
        }

        private static void UnRegister<T>(T tempCtrl) where T : PawnController
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
