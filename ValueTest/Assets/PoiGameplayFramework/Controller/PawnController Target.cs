using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
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


        /// <summary>
        /// 将输入命令解析为对Pawn命令（AI中使用行为树或状态机解析）
        /// </summary>
        /// <param name="next">输入的命令</param>
        /// <returns></returns>
        public void ParseInputCommand(InputCMD next)
        {
            if (Pawn == null) return;

            if (next.Jump)
            {
                Pawn.Jump();
            }

            ///解析所转向的角度
            Pawn.NextTurnToAngle = next.NextAngle ?? Pawn.NextTurnToAngle;

            ///计算移动
            Pawn.Acceleration = next.Acceleration;

            if (next.IsAttact)
            {
                 Pawn.Attack();
            }
        }
    }
}
