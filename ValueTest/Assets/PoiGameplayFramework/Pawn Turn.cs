using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    public partial class Pawn
    {
        /// <summary>
        /// 下一步需要旋转到的角度
        /// </summary>
        public float NextTurnToAngle { get;protected set; }


        public void TurnToAngle(float angle)
        {
            NextTurnToAngle = angle;
        }

        /// <summary>
        /// 自身旋转到目标角度
        /// </summary>
        protected virtual void ApplyTurn()
        {

            var delta = NextTurnToAngle - transform.localEulerAngles.y;

            if (delta == 0) return;

            if (delta > 180) delta -= 360;
            if (delta < -180) delta += 360;

            var tempSpeed = DataInfo.TurnSpeed * Time.fixedDeltaTime;

            delta = delta.ClampIn(-tempSpeed, tempSpeed);

            transform.Rotate(0, delta, 0, Space.World);
        }   
    }
}
