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
    public partial class PawnController
    {

        public ITarget Target { get; protected set; }

        /// <summary>
        /// 更新目标
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void UpdateTarget(float deltaTime)
        {

        }
    }
}
