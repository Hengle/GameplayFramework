using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Poi
{
    /// <summary>
    /// 角色基本移动功能
    /// </summary>
    [FunctionComponent]
    public class PawnMovement : MonoBehaviour, IMovement
    {
        /// <summary>
        /// 移动到目标点
        /// </summary>
        /// <param name="destination"></param>
        public void MoveTo(Vector2 destination)
        {

        }
        public void MoveTo(Vector3 destination)
        {

        }
    }
}
