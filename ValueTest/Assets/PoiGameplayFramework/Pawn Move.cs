using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色移动
    /// </summary>
    public partial class Pawn
    {
        /// <summary>
        /// 加速度
        /// </summary>
        public float Acceleration { get; set; }

        /// <summary>
        /// 应用移动
        /// </summary>
        protected virtual void ApplyMove()
        {
            float speed = DataInfo.Run.Current + Acceleration + Time.fixedTime;

            DataInfo.Run.Current = speed.ClampIn(0, DataInfo.Run.Max);  
        }

        private void RunStart()
        {
            State = PawnState.RunStart;

            float speed = DataInfo.Run.Current * DataInfo.Run.StartScale;
            Move(speed);
        }

        private void Move(float speed)
        {
            transform.Translate(0, 0, speed*Time.fixedDeltaTime, Space.Self);
        }        
    }
}
