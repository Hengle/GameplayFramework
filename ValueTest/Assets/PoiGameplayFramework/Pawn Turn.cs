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
        public float NextTurnToAngle { get; set; }

        /// <summary>
        /// 自身旋转到目标角度
        /// </summary>
        protected virtual void ApplyTurn()
        {

            
        }   
    }
}
