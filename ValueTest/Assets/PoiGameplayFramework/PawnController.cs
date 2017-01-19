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
    public partial class PawnController:MonoBehaviour
    {
        
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

        // 加载脚本实例时调用 Awake
        protected virtual void Awake()
        {

        }

        public virtual void Init() { }


        // 仅在首次调用 Update 方法之前调用 Start
        protected virtual void Start()
        {

        }

        // 当 MonoBehaviour 将被销毁时调用此函数
        protected virtual void OnDestroy()
        {
            Controllers.Remove(this);
        }

        // 如果 MonoBehaviour 已启用，则在每一帧都调用 Update
        protected virtual void Update()
        {

        }

    }
}
